﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IOrderService {
		public Task<ActionResult<string>> CheckAdress();
		public Task<ActionResult<OrderPagedList>> GetCustomerOrder(OrderFilterCustomer model);
		public Task<ActionResult<List<OrderDTO>>> GetCurrentOrder();
		public Task<ActionResult<Response>> CreateOrder(string? address, DateTime deliveryTime);
		public Task<ActionResult<Response>> ChangeOrderStatusCook(Guid orderId);
		public Task<ActionResult<Response>> ChangeOrderStatusCourier(Guid orderId);
		public Task<ActionResult<Response>> CancelOrderCustomer(Guid orderId);
		public Task<ActionResult<Response>> CancelOrderCourier(Guid orderId);
		public Task<ActionResult<OrderPagedList>> GetCourierOrdersHistory(OrderFilterCourier filter);
		public  Task<ActionResult<OrderPagedList>> GetCourierOrders(OrderFilterCourier filter);
		public Task<ActionResult<OrderPagedList>> GetCurrentCourier(OrderFilterCourier filter);
	}
}