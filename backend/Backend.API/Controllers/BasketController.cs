using System.Security.Claims;
using Azure;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response = Common.DTO.Response;

namespace Backend.API.Controllers {
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase {
		private readonly IBasketService _basketService;
		private readonly IPermissionCheckService _permissionService;


		public BasketController(IBasketService basketService, IPermissionCheckService permissionService) {
			_basketService= basketService;
			_permissionService = permissionService;
		}

		///<summary>
		///get user basket
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		public async Task<ActionResult<BasketDTO>> GetBasket() {
           return Ok( await _basketService.GetBasket(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
        }
		/// <summary>
		/// name of restaraunt , dihes of which are contained in basket
		/// </summary>
		[HttpGet]
		[Route("dish/check")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		public async Task<ActionResult<string>> CheckBasket() {
			return Ok(await _basketService.CheckBasketOnDishesFromOneRestaraunt(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		///<summary>
		///add dish to cart
		/// </summary>
		[HttpPost]
        [Route("dish/{dishId}")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]
		public async Task<ActionResult<Response>> AddDishToBasket(Guid dishId) {
			return Ok(await _basketService.AddDishToBasket(dishId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
		///<summary>
		///if "CompletelyDelete" = false then decrease number of dish , else remove dish completely
		/// </summary>
		[HttpDelete]
        [Route("dish/{dishId}")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]

		public async Task<ActionResult<Response>> RemoveDish(Guid dishId, bool CompletelyDelete) {
			return Ok(await _basketService.RemoveDish(dishId,new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value), CompletelyDelete));
		}
		///<summary>
		///remove all dishes from user's basket
		/// </summary>
		[HttpDelete]
		[Route("clearAll")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Customer)]

		public async Task<ActionResult<Response>> ClearBasket() {
			return Ok(await _basketService.ClearBasket(new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value)));
		}
	}
}
