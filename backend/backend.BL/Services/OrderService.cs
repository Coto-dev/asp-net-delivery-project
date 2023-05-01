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

namespace Backend.BL.Services {
	public class OrderService : IOrderService {
		private readonly ILogger<OrderService> _logger;
		private readonly BackendDbContext _context;
		public OrderService(ILogger<OrderService> logger, BackendDbContext context) {
			_logger = logger;
			_context = context;
		}
		public async Task<ActionResult<Response>> CancelOrderCustomer(Guid orderId , Guid customerId) {
			await CheckPermissionForCustomer(orderId, customerId);
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId );
			if (order == null) throw new KeyNotFoundException("заказ с таким id не найдено");
			order.Status = Statuses.Canceled;
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = "succesfully cancelled"
			};
		}
		public async Task<ActionResult<Response>> CancelOrderCourier(Guid orderId, Guid courierId) {
			await CheckPermissionForCourier(orderId, courierId);
			var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.Status == Statuses.Delivery);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			order.Status = Statuses.Canceled;
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = "succesfully cancelled"
			};
		}

		public async Task<ActionResult<Response>> ChangeOrderStatusCook(Guid orderId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> ChangeOrderStatusCourier(Guid orderId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<string>> CheckAdress(string address) {
			if (address == null) throw new NotFoundException("адресс пользователя не найден");
			else return address;
		}

		public async Task CheckPermissionForCook(Guid orderId, Guid cookId) {
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (order.CookerId == null) throw new NotFoundException("у этого заказа еще нет повара");
			if (order.CookerId != cookId) {
				throw new NotAllowedException("Этот заказ не принадлжит этому повару") ;
			}

		}
		public async Task CheckPermissionForCourier(Guid orderId, Guid courierId) {
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (order.CourId == null) throw new NotFoundException("у этого заказа еще нет курьера");
			if (order.CourId != courierId) {
				throw new NotAllowedException("Этот заказ не принадлжит этому курьеру");
			}
		}


		public async Task CheckPermissionForCustomer(Guid orderId, Guid customerId) {
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			var customer = await _context.Customers.Include(o => o.Orders).FirstOrDefaultAsync(o => o.Id == customerId);
			if (customer == null) throw new KeyNotFoundException("пользователя с таким id не найдено");
			if (!customer.Orders.Contains(order)) throw new NotAllowedException("Этот заказ не принадлжит этому пользователю");
		}
		/// <summary>
		/// TODO 
		/// </summary>
		public async Task<ActionResult<Response>> CreateOrder(string? address, DateTime deliveryTime, Guid customerId) {
			var customer = await _context.Customers.Include(x=>x.DishInCart).FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null) throw new KeyNotFoundException("Такой пользователь не найден");
			if (customer.DishInCart == null) throw new KeyNotFoundException("Корзина пользователя пуста");
			return null;
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
