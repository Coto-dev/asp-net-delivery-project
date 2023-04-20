using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers {
	[Route("api/order")]
	[ApiController]
	public class OrderController : ControllerBase {


		/// <summary>
		/// get information about customer orders history  
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Customer)]
		[Route("customer/ordersHistory")]
		public async Task<ActionResult<OrderPagedList>> GetCustomerOrder([FromQuery] OrderFilterCustomer model) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// get information about current order 
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		/// <remarks>
		/// current orders means orders where statuses are :"created","kitchen","IsReadyToDelivery","Delivery"
		/// </remarks>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Customer)]
		[Route("customer/current")]
		public async Task<ActionResult<List<OrderDTO>>> GetCurrentOrder() {
			throw new NotImplementedException(); //если у заказа статус от created до deliveried(не включая)
		}

		/// <summary>
		/// cancel order if only status created or delivery(courier)
		/// </summary>
		/// <remarks>
		/// if address will be null then address wil be taken from user profile
		/// </remarks>
		[HttpPost]
		[Authorize(Roles = ApplicationRoleNames.Customer)]
		[Route("create")]
		public async Task<ActionResult<Response>> CreateOrder(string? address, DateTime deliveryTime) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// get info about all ready to delivery orders for courier
		/// </summary>
		[HttpPost]
		[Authorize(Roles = ApplicationRoleNames.Customer)]
		[Route("customer/repeat/{orderId}")]
		public async Task<ActionResult<Response>> RepeatOrder(Guid orderId) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Change order status for cook
		/// </summary>
		/// <remarks>
		/// for cook “created” to “kitchen” to “readyToDelivery”
		/// </remarks>
		[HttpPut]
		[Authorize(Roles = ApplicationRoleNames.Cook)]
		[Route("cook/status/change/{orderId}")]
		public async Task<ActionResult<Response>> ChangeOrderStatusCook(Guid orderId) {
			throw new NotImplementedException();// 
		}
		/// <summary>
		/// Change order status for courier
		/// </summary>
		/// <remarks>
		/// for courier “readyToDelivery” to “Delivery” to “Delivered”
		/// </remarks>
		[HttpPut]
		[Authorize(Roles = ApplicationRoleNames.Courier)]
		[Route("courier/status/change/{orderId}")]
		public async Task<ActionResult<Response>> ChangeOrderStatusCourier(Guid orderId) {
			throw new NotImplementedException();// 
		}
		/// <summary>
		/// cancel order if only status created
		/// </summary>
		/// <remarks>
		/// created to cancelled
		/// </remarks>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoleNames.Customer)]

		[Route("{orderId}/customer/cancel")]
		public async Task<ActionResult<Response>> CancelOrderCustomer(Guid orderId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// cancel order if only status delivery
		/// </summary>
		/// <remarks>
		/// delivery to cancelled
		/// </remarks>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoleNames.Courier)]
		[Route("{orderId}/courier/cancel")]
		public async Task<ActionResult<Response>> CancelOrderCourier(Guid orderId) {
			throw new NotImplementedException();
		}


		/// <summary>
		/// get info about courier orders history
		/// </summary>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Courier)]
		[Route("courier/ordersHistory")]
		public async Task<ActionResult<OrderPagedList>> GetCourierOrdersHistory([FromQuery] OrderFilterCourier filter) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// get info about all ready to delivery orders for courier
		/// </summary>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Courier)]
		[Route("courier/orders/readyToDelivery")]
		public async Task<ActionResult<OrderPagedList>> GetCourierOrders([FromQuery] OrderFilterCourier filter) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// get info about current orders for courier
		/// </summary>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Courier)]
		[Route("courier/current")]
		public async Task<ActionResult<OrderPagedList>> GetCurrentCourier([FromQuery] OrderFilterCourier filter) {
			throw new NotImplementedException();
		}


	}
}
