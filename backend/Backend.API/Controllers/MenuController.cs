using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers {
	[Route("api/menu")]
	[ApiController]
	public class MenuController : ControllerBase {
		/// <summary>
		/// create menu in restaraunt for manager
		/// </summary>
		/// <remarks>
		/// you already have default empty menu after creating restaraunt
		/// </remarks>

		[HttpPost]
		[Authorize(Roles =ApplicationRoleNames.Manager)]
		[Route("restaraunt/{restarauntId}/create")]
		public async Task<Response> CreateMenu(Guid restarauntId, [FromBody] MenuShortModelDTO model) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// add dish to concrete menu for manager
		/// </summary>
		[HttpPost]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/restaraunt/{restarauntId}/dish/{dishId}/addDish")]
		public async Task<ActionResult<Response>> AddDishToMenu(Guid menuId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// delete dish from menu for manager
		/// </summary>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/restaraunt/{restarauntId}/dish/{dishId}/deleteDish")]
		public async Task<ActionResult<Response>> DeleteDishFromMenu(Guid menuId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// edit menu for manager
		/// </summary>
		[HttpPut]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/edit")]
		public async Task<ActionResult<Response>> EditMenu( Guid menuId, [FromBody] MenuShortModelDTO model) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// soft delete menu for manager
		/// </summary>
		[HttpDelete]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("{menuId}/delete")]
		public async Task<ActionResult<Response>> DeleteMenu(Guid menuId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// recover menu for manager
		/// </summary>
		[HttpPut]
		[Route("{menuId}/recover")]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		public async Task<ActionResult<Response>> RecoverMenu(Guid menuId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// get deleted menus for manager
		/// </summary>
		[HttpGet]
		[Authorize(Roles = ApplicationRoleNames.Manager)]
		[Route("restaraunt/{restarauntId}")]
		public async Task<ActionResult<MenuDTO>> GetDeletedMenus(Guid restarauntId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// get all existing not deleted menus
		/// </summary>
		[HttpGet]
		[Route("restaraunt/{restarauntId}/getAll")]
		public async Task<ActionResult<List<MenuShortDTO>>> GetMenus(Guid restarauntId) {
			throw new NotImplementedException();
		}
		/// <summary>
		/// get menu details with list of dishes
		/// </summary>
		[HttpGet]
		[Route("{menuId}/getDetails")]
		public async Task<ActionResult<MenuDTO>> GetMenuDetails(Guid menuId) {
			throw new NotImplementedException();
		}

	}
}
