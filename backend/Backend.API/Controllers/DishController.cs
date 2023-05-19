using System.Security.Claims;
using Backend.DAL.Data.Entities;
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
		private readonly IPermissionCheckService _permissionService;


		public DishController(ILogger<DishController> logger, IDishService dishService, IPermissionCheckService permissionService) {
			_logger = logger;
			_dishService = dishService;
			_permissionService = permissionService;
		}
		/// <summary>
		/// create new dish in concrete menu for manager
		/// </summary>
		[HttpPost]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("restaraunt/{restarauntId}/menu/{menuId}")]
		public async Task<ActionResult<Response>> CreateDish([FromBody] DishModelDTO model, Guid menuId , Guid restarauntId) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _dishService.CreateDishWithMenu(model, menuId,restarauntId));
		}
		/*/// <summary>
		/// create new dish in hidden menu for manager
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpPost]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("restaraunt/{restarauntId}/create")]
		public async Task<ActionResult<Response>> CreateDish([FromBody] DishModelDTO model, Guid restarauntId) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _dishService.CreateDishWithHiddenMenu(model, restarauntId));
		}*/
		/// <summary>
		/// Get all dishes from menu
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpGet]
		[Route("restaraunt/{restarauntId}/getAll")]
		public async Task<ActionResult<DishesPagedListDTO>> GetDishes([FromQuery] DishFilterModelDTO model, Guid restarauntId) {
			return Ok(await _dishService.GetDishes(model, restarauntId));
		}
		/// <summary>
		/// Get all deleted dishes from menu
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpGet]
		[Route("restaraunt/{restarauntId}/getDeleted")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]

		public async Task<ActionResult<DishesPagedListDTO>> GetDeletedDishes([FromQuery] DishFilterModelDTO model,Guid restarauntId) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _dishService.GetDeletedDishes(model, restarauntId));
		}


		/// <summary>
		/// Get dish details by id 
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		[HttpGet]
		[Route("{dishId}/getDetails")]
		public async Task<ActionResult<DishDetailsDTO>> GetDishDetails(Guid dishId) {
			return Ok(await _dishService.GetDishById(dishId));
		}

		/// <summary>
		/// add rating to dish 
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpPost]
		[Route("{dishId}/rating")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		public async Task<ActionResult<Response>> AddRatingToDish([FromBody] DishRatingDTO model) {
			return Ok(await _dishService.AddRatingToDish(model,  new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		[HttpGet]
		[Route("{dishId}/rating/check")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		public async Task<ActionResult<bool>> CheckRating(Guid dishId) {
			return Ok(await _dishService.CheckRating(dishId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}

		/// <summary>
		/// edit dish for manager
		/// </summary>
		[HttpPut]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("{dishId}/restaraunt/{restarauntId}/edit")]
		public async Task<ActionResult<Response>> EditDish(DishModelDTO model, Guid dishId , Guid restarauntId) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _dishService.EditDish(model, dishId));
		}

		/// <summary>
		/// soft delete dish for manager
		/// </summary>
		[HttpDelete]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("{dishId}/restaraunt/{restarauntId}/delete")]
		public async Task<ActionResult<Response>> DeleteDish(Guid dishId, Guid restarauntId) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _dishService.DeleteDish(dishId));
		}
		/// <summary>
		/// recover dish for manager
		/// </summary>
		[HttpPut]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("{dishId}/restaraunt/{restarauntId}/recover")]
		public async Task<ActionResult<Response>> RecoverDish(Guid dishId, Guid restarauntId) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			throw new NotImplementedException();
		}
	}
}
