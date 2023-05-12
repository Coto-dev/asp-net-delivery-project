using System.Net;
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
		[Route("customer/address")]
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
		[Route("customer/history")]
		public async Task<ActionResult<OrderPagedList>> GetCustomerOrderHistory([FromQuery] OrderFilter filter) {
			return Ok(await _orderService.GetOrderHistoryCustomer(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));

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
		public async Task<ActionResult<OrderPagedList>> GetCurrentOrder() {
			return Ok(await _orderService.GetCurrentOrderCustomer(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
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
		public async Task<ActionResult<Response>> CreateOrder(string address, DateTime deliveryTime) {
			return Ok(await _orderService.CreateOrder(address, deliveryTime, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}

		/// <summary>
		/// get info about all ready to delivery orders for courier
		/// </summary>
		[HttpPost]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		[Route("customer/repeat/{orderId}")]
		public async Task<ActionResult<Response>> RepeatOrder(string address, DateTime deliveryTime, Guid orderId) {
			await _permissionService.CheckPermissionForCustomer(orderId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _orderService.RepeatOrder(address, deliveryTime, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value),orderId));

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
		[Route("courier/history")]
		public async Task<ActionResult<OrderPagedList>> GetCourierOrdersHistory([FromQuery] OrderFilter filter) {
			return Ok(await _orderService.GetOrdersHistoryCourier(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}

		/// <summary>
		/// get info about all ready to delivery orders for courier
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Courier)]
		[Route("courier/readyToDelivery")]
		public async Task<ActionResult<OrderPagedList>> GetCourierOrders([FromQuery] OrderFilter filter) {
			return Ok(await _orderService.GetReadyToDeliveryOrdersCourier(filter));
		}
		/// <summary>
		/// get info about current orders for courier
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Courier)]
		[Route("courier/current")]
		public async Task<ActionResult<OrderPagedList>> GetCurrentCourier([FromQuery] OrderFilter filter) {
			return Ok(await _orderService.GetCurrentCourier(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		/// <summary>
		/// get info about created orders for cook
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Cook)]
		[Route("cook/created")]
		public async Task<ActionResult<OrderPagedList>> GetCreatedOrders([FromQuery] OrderFilter filter) {
			return Ok(await _orderService.GetCreatedOrdersCook(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		/// <summary>
		/// get info about history orders for concrete cook
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Cook)]
		[Route("cook/history")]
		public async Task<ActionResult<OrderPagedList>> GetOrdersHistoryCook([FromQuery] OrderFilter filter) {
			return Ok(await _orderService.GetOrdersHistoryCook(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		/// <summary>
		/// get info about current orders for concrete cook
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Cook)]
		[Route("cook/current")]
		public async Task<ActionResult<OrderPagedList>> GetOrdersCurrentCook([FromQuery] OrderFilter filter) {
			return Ok(await _orderService.GetOrdersCurrentCook(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		/// <summary>
		/// get info about all orders where manager's working
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("manager/all")]
		public async Task<ActionResult<OrderPagedList>> GetManagerOrders([FromQuery] OrderFilterManager filter) {
			return Ok(await _orderService.GetOrdersManager(filter, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}

	}
}
