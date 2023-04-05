using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Common.Enums;
using Backend.DAL.Data;
using Common.AuthInterfaces;
using Common.DTO;
using Common.Enums;
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

        public Task<Response> AddRatingToDish(Guid DishId, double value) {
            throw new NotImplementedException();
        }

        public Task<DishDetailsDTO> GetDishById(Guid id) {
            throw new NotImplementedException();
        }

        public Task<DishDetailsDTO> GetPagedListDishes(Categories Category, bool Vegetarian, DishSorting Sorting, int page, Guid RestarauntId) {
            throw new NotImplementedException();
        }
    }
}
