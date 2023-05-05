using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IBasketService {
		public Task<BasketDTO> GetBasket(Guid customerId);
		/// <summary>
		/// возвращает название ресторана, блюда которого на данный момент лежат в корзине
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns></returns>
		public Task<string> CheckBasketOndishesFromOneRestaraunt(Guid customerId);
		public Task<Response> AddDishToBasket(Guid dishId, Guid customerId);
		public  Task<Response> RemoveDish(Guid dishId, Guid customerId, bool CompletelyDelete);
		public Task<Response> ClearBasket(Guid customerId);
	}
}
