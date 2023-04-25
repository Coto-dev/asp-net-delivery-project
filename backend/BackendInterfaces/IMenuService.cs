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
		public Task<Response> AddDishToMenu(Guid menuId, Guid dishId);
		public Task<Response> DeleteDishFromMenu(Guid menuId, Guid dishId);
		public Task<Response> EditMenu(Guid menuId, MenuShortModelDTO model);
		public Task<Response> DeleteMenu(Guid menuId);
		public Task<Response> RecoverMenu(Guid menuId);
		public Task<List<MenuDTO>> GetDeletedMenus(Guid restarauntId);
		public Task<List<MenuShortDTO>> GetMenus(Guid restarauntId);
		public Task<MenuDishesPagedListDTO> GetMenuDetails(Guid menuId, int Page =1);
	}
}
