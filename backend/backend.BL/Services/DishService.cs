using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.BL.Services {

	public class DishService : IDishService {
        private readonly BackendDbContext _context;
        public DishService(BackendDbContext context) {
            _context = context;
        }

        public async Task<Response> AddRatingToDish(Guid dishId, double value, Guid userId) {
			var dish = await _context.Dishes
				.Include(x => x.Ratings)
				.ThenInclude(r=>r.Customer).
				FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Меню с с таким id не найдено");

			return null;
		}

		public async Task<bool> CheckRating(Guid dishId, Guid userId) {
			var dish = await _context.Dishes
				.Include(x => x.Ratings)
				.ThenInclude(r => r.Customer).
				FirstOrDefaultAsync(x => x.Id == dishId);
			var check = await _context.Orders.FirstOrDefaultAsync(o=>o.Status == Statuses.Deilvered && o.Dishes.DishesCart.Contains(dish));
			if (check == null) return false;
			return true;

		}

		public async Task<Response> CreateDish(DishModelDTO model, Guid restarauntId, Guid menuId) {
			throw new NotImplementedException();
		}

		public async Task<Response> CreateDish(Guid restarauntId) {
			throw new NotImplementedException();
		}

		public async Task<Response> DeleteDish(Guid dishId) {
			throw new NotImplementedException();
		}

		public async Task<Response> EditDish(DishModelDTO model) {
			throw new NotImplementedException();
		}

		public async Task<DishesPagedListDTO> GetDeletedDishes(DishFilterModelDTO model) {
			throw new NotImplementedException();
		}

		public async Task<DishDetailsDTO> GetDishById(Guid id) {
            throw new NotImplementedException();
        }

		public async Task<DishesPagedListDTO> GetGishes(DishFilterModelDTO model) {
			throw new NotImplementedException();
		}

		public async Task<DishDetailsDTO> GetPagedListDishes(Categories Category, bool Vegetarian, DishSorting Sorting, int page, Guid RestarauntId) {
            throw new NotImplementedException();
        }

		public async Task<Response> RecoverDish(Guid restarauntId) {
			throw new NotImplementedException();
		}
	}
}
