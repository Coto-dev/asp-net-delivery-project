using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Backend.BL.Services {
	public class OrderService : IOrderService {
		private readonly ILogger<OrderService> _logger;
		private readonly BackendDbContext _context;
		public OrderService(ILogger<OrderService> logger, BackendDbContext context) {
			_logger = logger;
			_context = context;
		}
		public async Task<ActionResult<Response>> CancelOrderCustomer(Guid orderId) {
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
		public async Task<ActionResult<Response>> CancelOrderCourier(Guid orderId) {
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

		public async Task<ActionResult<Response>> ChangeOrderStatusCook(Guid orderId, Guid cookId) {
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

		public async Task<ActionResult<Response>> ChangeOrderStatusCourier(Guid orderId, Guid courierId) {
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

		public async Task<ActionResult<string>> CheckAdress(string address) {
			if (address == null) throw new NotFoundException("адресс пользователя не найден");
			else return address;
		}

		/// <summary>
		/// TODO 
		/// </summary>
		public async Task<ActionResult<Response>> CreateOrder(string address, DateTime deliveryTime, Guid customerId) {
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

		public async Task<ActionResult<OrderPagedList>> GetCourierOrders(OrderFilterCourier filter) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<OrderPagedList>> GetCourierOrdersHistory(OrderFilterCourier filter) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<OrderPagedList>> GetCurrentCourier(OrderFilterCourier filter) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<List<OrderDTO>>> GetCurrentOrder() {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<OrderPagedList>> GetCustomerOrder(OrderFilterCustomer model) {
			throw new NotImplementedException();
		}
	}
}
