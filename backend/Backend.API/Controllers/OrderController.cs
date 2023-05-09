using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers {
	[Route("api/order")]
	[ApiController]
	public class OrderController : ControllerBase {
		private readonly ILogger<OrderController> _logger;
		private readonly IOrderService _orderService;
		private readonly IPermissionCheckService _permissionService;

		public OrderController(ILogger<OrderController> logger, IOrderService orderService, IPermissionCheckService checkPermission) {
			_logger = logger;
			_orderService = orderService;
			_permissionService = checkPermission;
		}
		/// <summary>
		/// return customer address if not null for customer 
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		[Route("customer/Address")]
		public async Task<ActionResult<string>> CheckAdress() {
			return Ok(await _orderService.CheckAdress(User.FindFirst(ClaimTypes.StreetAddress).Value));
		}
		/// <summary>
		/// get information about customer orders history  
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		/// 
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
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
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
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
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		[Route("create")]
		public async Task<ActionResult<Response>> CreateOrder(string? address, DateTime deliveryTime) {
			return Ok(await _orderService.CreateOrder(address, deliveryTime, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}

		/// <summary>
		/// get info about all ready to delivery orders for courier
		/// </summary>
		[HttpPost]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		[Route("customer/repeat/{orderId}")]
		public async Task<ActionResult<Response>> RepeatOrder(Guid orderId) {
			await _permissionService.CheckPermissionForCustomer(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));

			throw new NotImplementedException();
		}

		/// <summary>
		/// Change order status for cook
		/// </summary>
		/// <remarks>
		/// for cook “created” to “kitchen” to “readyToDelivery”
		/// </remarks>
		[HttpPut]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Cook)]
		[Route("cook/status/change/{orderId}")]
		public async Task<ActionResult<Response>> ChangeOrderStatusCook(Guid orderId) {
			await _permissionService.CheckPermissionForCook(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));

			return Ok(await _orderService.ChangeOrderStatusCook(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		/// <summary>
		/// Change order status for courier
		/// </summary>
		/// <remarks>
		/// for courier “readyToDelivery” to “Delivery” to “Delivered”
		/// </remarks>
		[HttpPut]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Courier)]
		[Route("courier/status/change/{orderId}")]
		public async Task<ActionResult<Response>> ChangeOrderStatusCourier(Guid orderId) {
			await _permissionService.CheckPermissionForCourier(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _orderService.ChangeOrderStatusCourier(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		/// <summary>
		/// cancel order if only status created
		/// </summary>
		/// <remarks>
		/// created to cancelled
		/// </remarks>
		[HttpDelete]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]

		[Route("{orderId}/customer/cancel")]
		public async Task<ActionResult<Response>> CancelOrderCustomer(Guid orderId) {
			await _permissionService.CheckPermissionForCustomer(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _orderService.CancelOrderCustomer(orderId));
		}
		/// <summary>
		/// cancel order if only status delivery
		/// </summary>
		/// <remarks>
		/// delivery to cancelled
		/// </remarks>
		[HttpDelete]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Courier)]
		[Route("{orderId}/courier/cancel")]
		public async Task<ActionResult<Response>> CancelOrderCourier(Guid orderId) {
			await _permissionService.CheckPermissionForCourier(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _orderService.CancelOrderCourier(orderId));
		}


		/// <summary>
		/// get info about courier orders history
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Courier)]
		[Route("courier/ordersHistory")]
		public async Task<ActionResult<OrderPagedList>> GetCourierOrdersHistory([FromQuery] OrderFilterCourier filter) {
			return Ok(await _orderService.GetCourierOrdersHistory(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}

		/// <summary>
		/// get info about all ready to delivery orders for courier
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Courier)]
		[Route("courier/orders/readyToDelivery")]
		public async Task<ActionResult<OrderPagedList>> GetCourierOrders([FromQuery] OrderFilterCourier filter) {
			return Ok(await _orderService.GetCourierReadyToDeliveryOrders(filter));
		}
		/// <summary>
		/// get info about current orders for courier
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Courier)]
		[Route("courier/current")]
		public async Task<ActionResult<OrderPagedList>> GetCurrentCourier([FromQuery] OrderFilterCourier filter) {
			return Ok(await _orderService.GetCurrentCourier(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}


	}
}
