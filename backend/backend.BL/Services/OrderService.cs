using System;
using System.Collections.Generic;
using System.Linq;
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
		public OrderService(ILogger<OrderService> logger, BackendDbContext context, IMapper mapper) {
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}
		public async Task<Response> CancelOrderCustomer(Guid orderId) {
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказ с таким id не найдено");
			if (!(order.Status == Statuses.Created)) 
				throw new NotAllowedException("Заказ нельзя отменить т.к, его статус: " + order.Status.ToString());
			order.Status = Statuses.Canceled;
			await _context.SaveChangesAsync();

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
			if (order.Status == Statuses.Created) order.CookerId = cookId;
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = $"succesfully changed status: {status} to {order.Status}"
			};
		}

		public async Task<Response> ChangeOrderStatusCourier(Guid orderId, Guid courierId) {
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (!(order.Status == Statuses.ReadyToDelivery | order.Status == Statuses.Delivery))
				throw new NotAllowedException("Статус заказа нельзя изменить т.к его статус: " + order.Status);
			var status = order.Status;
			order.Status = order.Status == Statuses.ReadyToDelivery ? Statuses.Delivery : Statuses.Deilvered;
			if (order.Status == Statuses.ReadyToDelivery) order.CourId = courierId;

			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = $"succesfully changed status: {status} to {order.Status}"
			};
		}

		public async Task<string> CheckAdress(string address) {
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
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "order uccesfully created"
			};
		}
		public async Task<OrderPagedList> GetCourierOrders(OrderFilterCourier filter, Guid? courierId, Guid? customerId, List<Statuses> statuses) {
			if (filter.Page <= 0) throw new BadRequestException("Неверно указана страница");

			var totalItems = await _context.Orders
				.Where(x =>
				(filter == null || x.OrderNumber == filter.OrderNumber)
				&& (courierId == null || x.CourId == courierId)
				&& (customerId == null || x.CourId == customerId)
				&&(statuses.Count == 0 || statuses.Contains(x.Status)))
				.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / AppConstants.OrderPage);

			if (totalPages < filter.Page && totalItems != 0) throw new BadRequestException("Неверно указана текущая страница");

			var orders = await _context.Orders
					.Include(x => x.Dishes)
					.ThenInclude(x => x.Dish)
					.Where(x =>
					(filter == null || x.OrderNumber == filter.OrderNumber)
					&& (courierId == null || x.CourId == courierId)
					&& (customerId == null || x.CourId == customerId)
					&& (statuses.Count == 0 || statuses.Contains(x.Status)))
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
				PageInfo = new PageInfoDTO()
			};
		}

		public async Task<OrderPagedList> GetCourierReadyToDeliveryOrders(OrderFilterCourier filter) {
			return await GetCourierOrders(filter,null,null,new List<Statuses>());
		}

		public async Task<OrderPagedList> GetCourierOrdersHistory(OrderFilterCourier filter, Guid courierId) {
			return await GetCourierOrders(filter, courierId,null, new List<Statuses>() { Statuses.Deilvered });

		}

		public async Task<OrderPagedList> GetCurrentCourier(OrderFilterCourier filter, Guid courierId) {
			return await GetCourierOrders(filter, courierId,null, new List<Statuses>() { Statuses.Delivery });
		}

		public async Task<OrderPagedList> GetCurrentCustomerOrder(Guid customerId) {
			return await GetCourierOrders(new OrderFilterCourier(), null, customerId, new List<Statuses>() {
				Statuses.Delivery,
				Statuses.Created,
				Statuses.Kitchen,
				Statuses.ReadyToDelivery});
		}

		public async Task<OrderPagedList> GetCustomerOrderHistory(OrderFilterCourier model, Guid customerId) {
			return await GetCourierOrders(model, null, customerId, new List<Statuses>() {
				Statuses.Canceled,
				Statuses.Deilvered,
				Statuses.Delivery,
				Statuses.Created,
				Statuses.Kitchen,
				Statuses.ReadyToDelivery});
		}
	}
}
