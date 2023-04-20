using Backend.DAL.Data.Entities;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers {
	/// <summary>
	/// Controller for get inforamation about restaraunt
	/// </summary>
	[Route("api/restaraunt")]
	[ApiController]
	public class RestarauntController : ControllerBase {


		/// <summary>
		/// Get all restaurant info about restaraunts
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpGet]
		public async Task<ActionResult<RestarauntDTO>> GetRestaraunts() {
			throw new NotImplementedException();

		}

		/// <summary>
		/// get info about created orders for cook
		/// </summary>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Cook)]
		[Route("{restarauntId}/cook/createdOrders")]
		public async Task<ActionResult<OrderPagedList>> GetCreatedOrders([FromQuery] OrderFilterCookCreated model,Guid  restarauntId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// get info about history orders for concrete cook
		/// </summary>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Cook)]
		[Route("{restarauntId}/cook/history")]
		public async Task<ActionResult<OrderPagedList>> GetOrdersHistoryCook([FromQuery] OrderFilterCook model,Guid restarauntId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// get info about all orders in single restaraunt for manager
		/// </summary>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{restarauntId}/manager/allOrders")]
		public async Task<ActionResult<OrderPagedList>> GetManagerOrders([FromQuery] OrderFilterManager model,Guid restarauntId) {
			throw new NotImplementedException();
		}


	}
}
