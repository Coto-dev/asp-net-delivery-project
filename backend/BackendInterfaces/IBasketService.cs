using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IBasketService {
		public Task<ActionResult<List<BasketDTO>>> GetBasket(Guid dishId);
		public Task<ActionResult<Response>> AddDishToBasket(Guid dishId);
		public  Task<ActionResult<Response>> RemoveDish(Guid dishId, bool CompletelyDelete);
		public Task<ActionResult<Response>> ClearBasket(Guid dishId, bool CompletelyDelete);
	}
}
