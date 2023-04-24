using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IMenuService {
		public Task<Response> CreateMenu(Guid restarauntId, MenuShortModelDTO model);
		public Task<ActionResult<Response>> AddDishToMenu(Guid menuId, Guid dishId);
		public Task<ActionResult<Response>> DeleteDishFromMenu(Guid menuId, Guid dishId);
		public Task<ActionResult<Response>> EditMenu(Guid menuId, MenuShortModelDTO model);
		public Task<ActionResult<Response>> DeleteMenu(Guid menuId);
		public Task<ActionResult<Response>> RecoverMenu(Guid menuId);
		public Task<ActionResult<List<MenuDTO>>> GetDeletedMenus(Guid restarauntId);
		public Task<ActionResult<List<MenuShortDTO>>> GetMenus(Guid restarauntId);
		public Task<ActionResult<MenuDishesPagedListDTO>> GetMenuDetails(Guid menuId, int Page =1);
	}
}
