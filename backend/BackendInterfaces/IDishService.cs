using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IDishService {
		public Task<DishDetailsDTO> GetDishById(Guid id);
		public  Task<DishesPagedListDTO> GetGishes(DishFilterModelDTO model);
		public Task<DishesPagedListDTO> GetDeletedDishes(DishFilterModelDTO model);
		public Task<Response> AddRatingToDish(Guid dishId, double value , Guid userId);
		public Task<Response> CreateDish( DishModelDTO model, Guid restarauntId, Guid menuId);
		public Task<Response> CreateDish(Guid restarauntId);
		public Task<bool> CheckRating(Guid dishId, Guid userId);
		public Task<Response> EditDish(DishModelDTO model);
		public Task<Response> DeleteDish(Guid dishId);
		public Task<Response> RecoverDish(Guid restarauntId);
	}
}