using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IDishService {
		public Task<DishDetailsDTO> GetDishById(Guid id);
		public  Task<ActionResult<DishesPagedListDTO>> GetGishes(DishFilterModelDTO model);
		public Task<ActionResult<DishesPagedListDTO>> GetDeletedDishes(DishFilterModelDTO model);
		public Task<Response> AddRatingToDish(Guid DishId, double value);
		public Task<ActionResult<Response>> CreateDish( DishModelDTO model, Guid restarauntId, Guid menuId);
		public Task<ActionResult<Response>> CreateDish(Guid restarauntId);
		public Task<ActionResult<RatingDTO>> CheckRating(Guid Dishid);
		public Task<ActionResult<Response>> EditDish(DishModelDTO model);
		public Task<ActionResult<Response>> DeleteDish(Guid dishId);
		public Task<ActionResult<Response>> RecoverDish(Guid restarauntId);
	}
}