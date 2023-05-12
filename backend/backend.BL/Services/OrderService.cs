using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Backend.DAL.Data;
using Backend.DAL.Data.Entities;
using Backend.DAL.Migrations;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Common.Exceptions;
using CoomonThings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Backend.BL.Services {
	public class OrderService : IOrderService {
		private readonly ILogger<OrderService> _logger;
		private readonly BackendDbContext _context;
		private readonly IMapper _mapper;
		private readonly IRabbitMQService _rabbitmqService;
		public OrderService(ILogger<OrderService> logger, BackendDbContext context, IMapper mapper, IRabbitMQService rabbitmqService) {
			_logger = logger;
			_context = context;
			_mapper = mapper;
			_rabbitmqService = rabbitmqService;
		}
		public async Task<Response> CancelOrderCustomer(Guid orderId) {
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказ с таким id не найдено");
			if (!(order.Status == Statuses.Created)) 
				throw new NotAllowedException("Заказ нельзя отменить т.к, его статус: " + order.Status.ToString());
			order.Status = Statuses.Canceled;
			await _context.SaveChangesAsync();
			var message = $"Order cancelled";
			_rabbitmqService.SendMessage(new OrderChangeStatusMessage {
				description = message,
				orderId = orderId.ToString(),
				userId = orderId.ToString(),

			});
			return new Response {
				Status = "200",
				Message = "succesfully cancelled"
			};
		}
		public async Task<Response> CancelOrderCourier(Guid orderId) {
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (!(order.Status == Statuses.Delivery)) 
				throw new NotAllowedException("Заказ нельзя отменить т.к, его статус: " + order.Status);
			order.Status = Statuses.Canceled;
			await _context.SaveChangesAsync();
			var message = $"Order cancelled";
			_rabbitmqService.SendMessage(new OrderChangeStatusMessage {
				description = message,
				orderId = orderId.ToString(),
				userId = orderId.ToString(),

			});
			return new Response {
				Status = "200",
				Message = "succesfully cancelled"
			};
		}

		public async Task<Response> ChangeOrderStatusCook(Guid orderId, Guid cookId) {
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (!(order.Status == Statuses.Created | order.Status == Statuses.Kitchen))
				throw new NotAllowedException("Статус заказа нельзя изменить т.к его статус: " + order.Status.ToString());
			var status = order.Status;
			order.Status = order.Status == Statuses.Kitchen ? Statuses.ReadyToDelivery : Statuses.Kitchen;
			if (status == Statuses.Created) order.CookerId = cookId;
			await _context.SaveChangesAsync();
			var message = $"Статус заказа изменен: с {status} на {order.Status}";
			_rabbitmqService.SendMessage(new OrderChangeStatusMessage { 
				description = message,
				orderId = orderId.ToString(),
				userId= orderId.ToString(),

			});

			return new Response {
				Status = "200",
				Message = message
			};
		}

		public async Task<Response> ChangeOrderStatusCourier(Guid orderId, Guid courierId) {
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (!(order.Status == Statuses.ReadyToDelivery | order.Status == Statuses.Delivery))
				throw new NotAllowedException("Статус заказа нельзя изменить т.к его статус: " + order.Status);
			var status = order.Status;
			order.Status = order.Status == Statuses.ReadyToDelivery ? Statuses.Delivery : Statuses.Deilvered;
			if (status == Statuses.ReadyToDelivery) order.CourId = courierId;

			await _context.SaveChangesAsync();
			var message = $"Статус заказа изменен: с {status} на {order.Status}";
			_rabbitmqService.SendMessage(new OrderChangeStatusMessage {
				description = message,
				orderId = orderId.ToString(),
				userId = orderId.ToString(),

			});

			return new Response {
				Status = "200",
				Message = message
			};
		}

		public async Task<string> CheckAdress(string address) {
			_rabbitmqService.SendMessage(address);
			if (address == null) throw new NotFoundException("адресс пользователя не найден");
			else return address;
		}

		/// <summary>
		/// TODO 
		/// </summary>
		public async Task<Response> CreateOrder(string address, DateTime deliveryTime, Guid customerId) {
			var customer = await _context.Customers.Include(x=>x.DishInCart).ThenInclude(d=>d.Dish).FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null) throw new KeyNotFoundException("Такой пользователь не найден");
			if (customer.DishInCart.IsNullOrEmpty()) throw new NotFoundException("Корзина пользователя пуста");
			if (!(deliveryTime > DateTime.Now.AddHours(1))) throw new BadRequestException("Дата доставки должна быть не раньше чем через час от текущего времени");

			var order = new Order {
				Address = address,
				DeliveryTime = deliveryTime,
				Customer = customer,
				Dishes = customer.DishInCart
				.Select(x => new DishInOrder {
					Dish = x.Dish,
					Count = x.Count
				}).ToList(),
				OrderTime = DateTime.Now,
				Price = customer.DishInCart.Sum(x => x.Dish.Price),
				Status = Statuses.Created,
			};
			await _context.Orders.AddAsync(order);
			customer.DishInCart.Clear();
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "order succesfully created"
			};
		}
		public async Task<OrderPagedList> GetOrders(OrderFilter filter, Guid? courierId, Guid? customerId, List<Statuses> statuses, Guid? restarauntId, Guid? cookId) {
			if (filter.Page <= 0) throw new BadRequestException("Неверно указана страница");

			var totalItems = await _context.Orders
				.Where(x =>
				(filter.OrderNumber == null || x.OrderNumber == filter.OrderNumber)
				&& (courierId == null || x.CourId == courierId)
				&& (customerId == null || x.Customer.Id == customerId)
				&& (cookId == null || x.CookerId == cookId)
				&& (statuses.Count == 0 || statuses.Contains(x.Status))
				&& (restarauntId == null || x.Dishes.Any(d=>d.Dish.Menus.Any(m=>m.Restaraunt.Id == restarauntId))))
				.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / AppConstants.OrderPage);

			if (totalPages < filter.Page && totalItems != 0) throw new BadRequestException("Неверно указана текущая страница");

			var orders = await _context.Orders
					.Include(x => x.Dishes)
					.ThenInclude(x => x.Dish)
					.Where(x =>
					(filter.OrderNumber == null || x.OrderNumber == filter.OrderNumber)
					&& (courierId == null || x.CourId == courierId)
					&& (customerId == null || x.Customer.Id == customerId)
					&& (cookId == null || x.CookerId == cookId)
					&& (statuses.Count == 0 || statuses.Contains(x.Status))
					&& (restarauntId == null || x.Dishes.Any(d => d.Dish.Menus.Any(m => m.Restaraunt.Id == restarauntId))))
				   .Select(x => x)
				   .Skip((filter.Page - 1) * AppConstants.OrderPage)
				   .Take(AppConstants.OrderPage)
				   .ToListAsync();

			var ordersDTO = orders.Select(x => new OrderDTO {
				Address = x.Address,
				DeliveryTime = x.DeliveryTime,
				OrderTime = x.OrderTime,
				Id = x.Id,
				OrderNumber = x.OrderNumber,
				Price = x.Price,
				Status = x.Status,
				Dishes = x.Dishes.Select(x => new DishShortModelDTO {
					Count = x.Count,
					Id = x.Dish.Id,
					Name = x.Dish.Name,
					PhotoUrl = x.Dish.PhotoUrl,
					Price = x.Dish.Price,
					TotalPrice = x.Dish.Price
				}).ToList()
			}).ToList();
			switch (filter.SortingDate) {
				case DateSorting.DeliveryDateAsc:
					orders.OrderBy(x => x.DeliveryTime);
					break;
				case DateSorting.DeliveryDateDesc:
					orders.OrderByDescending(x => x.DeliveryTime);
					break;
				case DateSorting.OrderDateAsc:
					orders.OrderBy(x => x.OrderTime);
					break;
				case DateSorting.OrderDateDesc:
					orders.OrderByDescending(x => x.OrderTime);
					break;
			}
			return new OrderPagedList {
				Orders = ordersDTO,
				PageInfo = new PageInfoDTO {
					Count = totalPages,
					Current = filter.Page,
					Size = AppConstants.OrderPage
				}
			};
		}

		public async Task<OrderPagedList> GetReadyToDeliveryOrdersCourier(OrderFilter filter) {
			return await GetOrders(filter,null,null,new List<Statuses>() { Statuses.ReadyToDelivery }, null,null);
		}

		public async Task<OrderPagedList> GetOrdersHistoryCourier(OrderFilter filter, Guid courierId) {
			return await GetOrders(filter, courierId,null, new List<Statuses>() { Statuses.Deilvered }, null, null);

		}

		public async Task<OrderPagedList> GetCurrentCourier(OrderFilter filter, Guid courierId) {
			return await GetOrders(filter, courierId,null, new List<Statuses>() { Statuses.Delivery }, null, null);
		}

		public async Task<OrderPagedList> GetCurrentOrderCustomer(Guid customerId) {
			return await GetOrders(new OrderFilter(), null, customerId, new List<Statuses>() {
				Statuses.Delivery,
				Statuses.Created,
				Statuses.Kitchen,
				Statuses.ReadyToDelivery},
				null, null);
		}

		public async Task<OrderPagedList> GetOrderHistoryCustomer(OrderFilter model, Guid customerId) {
			return await GetOrders(model, null, customerId, new List<Statuses>() {
				Statuses.Canceled,
				Statuses.Deilvered 
			},
				null, null);
		}

		public async Task<OrderPagedList> GetCreatedOrdersCook(OrderFilter model, Guid cookId) {
			var restaraunt = await _context.Restaraunts.FirstOrDefaultAsync(x=>x.Cooks.Any(x=>x.Id == cookId));
			if (restaraunt == null) throw new NotFoundException("этот повар не работает в ресторане");
			return await GetOrders(model, null, null, new List<Statuses>() { Statuses.Created }, restaraunt.Id, null);

		}

		public async Task<OrderPagedList> GetOrdersHistoryCook(OrderFilter model, Guid cookId) {
			return await GetOrders(model, null, null, new List<Statuses>() { Statuses.Kitchen,
			Statuses.ReadyToDelivery,
			Statuses.Canceled,
			Statuses.Deilvered,
			Statuses.Delivery
			}, null, cookId);
		}

		public async Task<OrderPagedList> GetOrdersCurrentCook(OrderFilter model, Guid cookId) {
			var restaraunt = await _context.Restaraunts.FirstOrDefaultAsync(x => x.Cooks.Any(x => x.Id == cookId));
			if (restaraunt == null) throw new NotFoundException("этот повар не работает в ресторане");
			return await GetOrders(model, null, null, new List<Statuses>() {
				Statuses.Kitchen,
			}, restaraunt.Id, cookId);
		}

		public async Task<OrderPagedList> GetOrdersManager(OrderFilterManager model, Guid managerId) {
			var restaraunt = await _context.Restaraunts.FirstOrDefaultAsync(x => x.Managers.Any(x => x.Id == managerId));
			if (restaraunt == null) throw new NotFoundException("этот менеджер не работает в ресторане");
			return await GetOrders(new OrderFilter { 
				OrderNumber = model.OrderNumber,
				Page= model.Page,
				SortingDate = model.SortingDate,
			}, null, null, model.Statuses, restaraunt.Id, null);
		}

		public async Task<Response> RepeatOrder(string address, DateTime deliveryTime, Guid customerId, Guid orderId) {
			var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null) throw new KeyNotFoundException("такой пользователь не найден");
			var order = await _context.Orders
				.Include(x=>x.Customer)
				.Include(x => x.Dishes)
				.ThenInclude(d => d.Dish)
				.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order == null) throw new KeyNotFoundException("такой заказ не найден");
			if (order.Customer.Id != customerId) throw new NotAllowedException("этот заказ не принадлежит этому пользователю");
			var NewOrder = new Order {
				Address = address,
				DeliveryTime = deliveryTime,
				Customer = customer,
				Dishes = order.Dishes.Select(x => new DishInOrder {
					Dish = x.Dish,
					Count = x.Count
				}).ToList(),
				OrderTime = DateTime.Now,
				Price = order.Price,
				Status = Statuses.Created,
			};
			await _context.Orders.AddAsync(NewOrder);
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "order succesfully created"
			};
		}
	}
}
