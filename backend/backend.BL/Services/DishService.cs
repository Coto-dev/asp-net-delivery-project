using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.BL.Services {

	public class DishService : IDishService {
        private readonly ILogger<DishService> _logger;
        private readonly BackendDbContext _context;
        public DishService(ILogger<DishService> logger, BackendDbContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task<Response> AddRatingToDish(Guid DishId, double value) {
            throw new NotImplementedException();
        }

		public async Task<ActionResult<RatingDTO>> CheckRating(Guid Dishid) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> CreateDish(DishModelDTO model, Guid restarauntId, Guid menuId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> CreateDish(Guid restarauntId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> DeleteDish(Guid dishId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> EditDish(DishModelDTO model) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<DishesPagedListDTO>> GetDeletedDishes(DishFilterModelDTO model) {
			throw new NotImplementedException();
		}

		public async Task<DishDetailsDTO> GetDishById(Guid id) {
            throw new NotImplementedException();
        }

		public async Task<ActionResult<DishesPagedListDTO>> GetGishes(DishFilterModelDTO model) {
			throw new NotImplementedException();
		}

		public async Task<DishDetailsDTO> GetPagedListDishes(Categories Category, bool Vegetarian, DishSorting Sorting, int page, Guid RestarauntId) {
            throw new NotImplementedException();
        }

		public async Task<ActionResult<Response>> RecoverDish(Guid restarauntId) {
			throw new NotImplementedException();
		}
	}
}
