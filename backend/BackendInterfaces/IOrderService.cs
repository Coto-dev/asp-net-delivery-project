using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IOrderService {
		public Task<string> CheckAdress(string address);
		public Task<OrderPagedList> GetOrderHistoryCustomer(OrderFilter model, Guid customerId);
		public Task<OrderPagedList> GetCurrentOrderCustomer(Guid customerId);
		public Task<Response> CreateOrder(string address, DateTime deliveryTime, Guid customerId);
		public Task<Response> RepeatOrder(string address, DateTime deliveryTime, Guid customerId, Guid orderId);
		public Task<Response> ChangeOrderStatusCook(Guid orderId, Guid cookId);
		public Task<Response> ChangeOrderStatusCourier(Guid orderId, Guid courierId);
		public Task<Response> CancelOrderCustomer(Guid orderId);
		public Task<Response> CancelOrderCourier(Guid orderId);
		public Task<OrderPagedList> GetOrdersHistoryCourier(OrderFilter filter, Guid courierId);
		public  Task<OrderPagedList> GetReadyToDeliveryOrdersCourier(OrderFilter filter);
		public Task<OrderPagedList> GetCurrentCourier(OrderFilter filter, Guid courierId);
		public Task<OrderPagedList> GetCreatedOrdersCook(OrderFilter model, Guid cookId);
		public Task<OrderPagedList> GetOrdersHistoryCook(OrderFilter model, Guid cookId);
		public Task<OrderPagedList> GetOrdersCurrentCook(OrderFilter model, Guid cookId);
		public Task<OrderPagedList> GetOrdersManager(OrderFilterManager model, Guid managerId);
	}
}
