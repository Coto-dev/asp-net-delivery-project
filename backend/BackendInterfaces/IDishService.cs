using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IDishService {
		public Task<DishDetailsDTO> GetDishById(Guid id);
		public  Task<DishesPagedListDTO> GetDishes(DishFilterModelDTO model, Guid restarauntId);
		public Task<DishesPagedListDTO> GetDeletedDishes(DishFilterModelDTO model, Guid restarauntId);
		//public Task<DishesPagedListDTO> GetExistingDishes(DishFilterModelDTO model, Guid restarauntId, bool deleted = false);
		public Task<Response> AddRatingToDish(DishRatingDTO model, Guid userId);
		public Task<Response> CreateDishWithMenu( DishModelDTO model, Guid menuId, Guid restarauntId);
		//public Task<Response> CreateDishWithHiddenMenu(DishModelDTO model, Guid restarauntId);
		public Task<bool> CheckRating(Guid dishId, Guid userId);
		public Task<Response> EditDish(DishModelDTO model, Guid dishId);
		public Task<Response> DeleteDish(Guid dishId);
		public Task<Response> RecoverDish(Guid dishId);
		//public Task CheckPermissionForManager(Guid restarauntId, Guid managerId);

	}
}