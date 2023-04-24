using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Common.BackendInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.BL.Services {
	public class OrderService : IOrderService {
		private readonly ILogger<OrderService> _logger;
		private readonly BackendDbContext _context;
		public OrderService(ILogger<OrderService> logger, BackendDbContext context) {
			_logger = logger;
			_context = context;
		}
		public async Task<ActionResult<Response>> CancelOrderCourier(Guid orderId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> CancelOrderCustomer(Guid orderId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> ChangeOrderStatusCook(Guid orderId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> ChangeOrderStatusCourier(Guid orderId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<string>> CheckAdress() {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> CreateOrder(string? address, DateTime deliveryTime) {
			throw new NotImplementedException();
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
