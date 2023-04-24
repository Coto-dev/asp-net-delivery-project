using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers {
	/// <summary>
	/// Controller for dish managment
	/// </summary>
	[Route("api/dish")]
	[ApiController]
	public class DishController : ControllerBase {

		private readonly ILogger<DishController> _logger;
		private readonly IDishService _dishService;

		public DishController(ILogger<DishController> logger, IDishService scheduleService) {
			_logger = logger;
			_dishService = scheduleService;

		}
		/// <summary>
		/// create new dish in concrete menu for manager
		/// </summary>
		[HttpPost]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{id}/restaraunt/{restarauntId}/menu/{menuId}")]
		public async Task<ActionResult<Response>> CreateDish([FromBody] DishModelDTO model, Guid restarauntId, Guid menuId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// create new dish in hidden menu for manager
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpPost]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("restaraunt/{restarauntId}/create")]
		public async Task<ActionResult<Response>> CreateDish(Guid restarauntId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// Get all dishes from menu
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpGet]
		[Route("restaraunt/{restarauntId}/getAll")]
		public async Task<ActionResult<DishesPagedListDTO>> GetDishes([FromQuery] DishFilterModelDTO model) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// Get all dishes from menu
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpGet]
		[Route("restaraunt/{restarauntId}/getDeleted")]
		public async Task<ActionResult<DishesPagedListDTO>> GetDeletedDishes([FromQuery] DishFilterModelDTO model) {
			throw new NotImplementedException();
		}


		/// <summary>
		/// Get dish details by id 
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		[HttpGet]
		[Route("{dishId}/getDetails")]
		public async Task<ActionResult<DishDetailsDTO>> GetGishDetails(Guid dishId) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// add rating to dish 
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpPost]
		[Route("{dishId}/rating")]
		public async Task<ActionResult<RatingDTO>> AddRatingToDish(Guid dishId, double value) {
			throw new NotImplementedException();
		}
		[HttpGet]
		[Route("{dishId}/rating/check")]
		public async Task<ActionResult<RatingDTO>> CheckRating(Guid dishId) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// edit dish for manager
		/// </summary>
		[HttpPut]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{dishId}/edit")]
		public async Task<ActionResult<Response>> EditDish(DishModelDTO model) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// soft delete dish for manager
		/// </summary>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{dishId}/delete")]
		public async Task<ActionResult<Response>> DeleteDish(Guid dishId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// recover dish for manager
		/// </summary>
		[HttpPut]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{dishId}/recover")]
		public async Task<ActionResult<Response>> RecoverDish(Guid restarauntId) {
			throw new NotImplementedException();
		}
	}
}
