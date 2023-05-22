using System.Security.Claims;
using Backend.BL.Services;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers {
	[Route("api/menu")]
	[ApiController]
	public class MenuController : ControllerBase {
		private readonly IMenuService _menuService;
		private readonly IPermissionCheckService _permissionService;

		public MenuController(IMenuService menuService, IPermissionCheckService checkPermission) {
			_permissionService = checkPermission;
			_menuService = menuService;

		}
		/// <summary>
		/// create menu in restaraunt for manager
		/// </summary>

		[HttpPost]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("restaraunt/{restarauntId}/create")]
		public async Task<ActionResult<Response>> CreateMenu(Guid restarauntId, [FromBody] MenuShortModelDTO model) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _menuService.CreateMenu(restarauntId, model));
		}
		/// <summary>
		/// add dish to concrete menu for manager
		/// </summary>
		[HttpPost]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/dish/{dishId}/addDish")]
		public async Task<ActionResult<Response>> AddDishToMenu(Guid menuId, Guid dishId) {
			await _permissionService.CheckPermissionForManagerByMenu(menuId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _menuService.AddDishToMenu(menuId, dishId));
		}
		/// <summary>
		/// delete dish from menu for manager
		/// </summary>
		[HttpDelete]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/dish/{dishId}/deleteDish")]
		public async Task<ActionResult<Response>> DeleteDishFromMenu(Guid menuId, Guid dishId) {
			await _permissionService.CheckPermissionForManagerByMenu(menuId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _menuService.DeleteDishFromMenu(menuId, dishId));
		}
		/// <summary>
		/// edit menu for manager
		/// </summary>
		[HttpPut]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/edit")]
		public async Task<ActionResult<Response>> EditMenu(Guid menuId, [FromBody] MenuShortModelDTO model) {
			await _permissionService.CheckPermissionForManagerByMenu(menuId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _menuService.EditMenu(menuId, model));
		}
		/// <summary>
		/// soft delete menu for manager
		/// </summary>
		[HttpDelete]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/delete")]
		public async Task<ActionResult<Response>> DeleteMenu(Guid menuId) {
			await _permissionService.CheckPermissionForManagerByMenu(menuId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _menuService.DeleteMenu(menuId));
		}
		/// <summary>
		/// recover menu for manager
		/// </summary>
		[HttpPut]
		[Route("{menuId}/recover")]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		public async Task<ActionResult<Response>> RecoverMenu(Guid menuId) {
			await _permissionService.CheckPermissionForManagerByMenu(menuId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _menuService.RecoverMenu(menuId));
		}
		/// <summary>
		/// get deleted menus for manager
		/// </summary>
		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer", Roles = ApplicationRoleNames.Manager)]
		[Route("restaraunt/{restarauntId}")]
		public async Task<ActionResult<MenuDTO>> GetDeletedMenus(Guid restarauntId) {
			await _permissionService.CheckPermissionForManagerByRestaraunt(restarauntId, new Guid(User.FindFirst(ClaimTypes.NameIdentifier).Value));
			return Ok(await _menuService.GetDeletedMenus(restarauntId));
		}
		/// <summary>
		/// get all existing not deleted menus
		/// </summary>
		[HttpGet]
		[Route("restaraunt/{restarauntId}/getAll")]
		public async Task<ActionResult<List<MenuShortDTO>>> GetMenus(Guid restarauntId) {
			return Ok(await _menuService.GetMenus(restarauntId));
		}


	}
}
