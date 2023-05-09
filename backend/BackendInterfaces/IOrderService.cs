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
		public Task<OrderPagedList> GetCustomerOrderHistory(OrderFilterCourier model, Guid customerId);
		public Task<OrderPagedList> GetCurrentCustomerOrder(Guid customerId);
		public Task<Response> CreateOrder(string address, DateTime deliveryTime, Guid customerId);
		public Task<Response> ChangeOrderStatusCook(Guid orderId, Guid cookId);
		public Task<Response> ChangeOrderStatusCourier(Guid orderId, Guid courierId);
/*		public Task CheckPermissionForCook(Guid orderId, Guid cookId);
		public Task CheckPermissionForCourier(Guid orderId, Guid courierId);
		public Task CheckPermissionForCustomer(Guid orderId, Guid customerId);*/
		public Task<Response> CancelOrderCustomer(Guid orderId);
		public Task<Response> CancelOrderCourier(Guid orderId);
		public Task<OrderPagedList> GetCourierOrdersHistory(OrderFilterCourier filter, Guid courierId);
		public  Task<OrderPagedList> GetCourierReadyToDeliveryOrders(OrderFilterCourier filter);
		public Task<OrderPagedList> GetCurrentCourier(OrderFilterCourier filter, Guid courierId);
	}
}
