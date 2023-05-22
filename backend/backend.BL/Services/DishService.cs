using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DAL.Data;
using Backend.DAL.Data.Entities;
using Backend.DAL.Migrations;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Common.Exceptions;
using CoomonThings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Backend.BL.Services {

	public class DishService : IDishService {
		private readonly ILogger<DishService> _logger;
        private readonly BackendDbContext _context;
		private readonly IMapper _mapper;
		public DishService(BackendDbContext context, IMapper mapper, ILogger<DishService> logger) {
            _context = context;
			_mapper = mapper;
			_logger = logger;
        }

        public async Task<Response> AddRatingToDish(DishRatingDTO model,Guid dishId, Guid userId) {
			var dish = await _context.Dishes
				.Include(x => x.Ratings)
				.ThenInclude(r=>r.Customer)
				.FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с с таким id не найдено");
			if (dish.DeletedTime.HasValue) throw new NotAllowedException("Блюдо удалено");
			if (! await CheckRating(dishId, userId)) throw new NotAllowedException("Пользователь с таким id не может поставить рейтинг");
			var userRating = await _context.Ratings.FirstOrDefaultAsync(x=>x.CustomerId == userId && x.Dish.Id == dishId);
			if (userRating != null) {
				userRating.Value = model.value;
				_context.Update(userRating);
			}
			else {
				var newRating = new Rating {
					Dish = dish,
					Value = model.value,
					CustomerId = userId
				};
				await _context.AddRangeAsync(newRating);
			}
			await _context.SaveChangesAsync();
			return new Response {
				Message = "Succesfully added",
				Status = "200"
			};
		}

		public async Task<bool> CheckRating(Guid dishId, Guid userId) {
			var dish = await _context.Dishes
				.Include(x => x.Ratings)
				.ThenInclude(r => r.Customer)
				.FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с с таким id не найдено");
			var check = await _context.Orders
				.FirstOrDefaultAsync(o=>o.Status == Statuses.Deilvered
				&& o.Dishes.Any(d=>d.Dish.Id == dishId)
				&& o.Customer.Id == userId);
			if (check == null) return false;
			return true;

		}

		public async Task<Response> CreateDishWithMenu(DishModelDTO model, Guid menuId, Guid restarauntId) {
			var menu = await _context.Menus.Include(m=>m.Dishes).FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с таким id не найдено");
			var rest = await _context.Restaraunts
				.Include(x => x.Menus)
				.FirstOrDefaultAsync(x => x.Id == restarauntId);
			if (!rest.Menus.Contains(menu)) throw new NotAllowedException("Это меню не принадлежит этому ресторану");

			var dish =  _mapper.Map<Dish>(model);
			await _context.Dishes.AddAsync(dish);
			menu.Dishes.Add(dish);
			await _context.SaveChangesAsync();
			return new Response {
				Message = "Succesfully created",
				Status = "200"
			};
		}


		public async Task<Response> DeleteDish(Guid dishId) {
			var dish = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с таким id не найдено");
			dish.DeletedTime = DateTime.Now;
			await _context.SaveChangesAsync();
			return new Response {
				Message = "Succesfully soft deleted",
				Status = "200"
			};
		}

		public async Task<Response> EditDish(DishModelDTO model, Guid dishId) {
			var dish = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с таким id не найдено");

			dish.Category = model.Category;
			dish.PhotoUrl = model.PhotoUrl;
			dish.Name = model.Name;
			dish.Price = model.Price;
			dish.Description = model.Description;
			dish.IsVagetarian = model.IsVagetarian;
			
			await _context.SaveChangesAsync();
			return new Response {
				Message = "Succesfully edited",
				Status = "200"
			};
		}

		public async Task<DishesPagedListDTO> GetDeletedDishes(DishFilterModelDTO model,Guid restarauntId) {
			return await GetExistingDishes(model, restarauntId, true);

		}

		public async Task<DishDetailsDTO> GetDishById(Guid id) {
           var dish =  await _context.Dishes.Include(x=>x.Ratings).FirstOrDefaultAsync(x=>x.Id == id);
			if (dish == null) throw new KeyNotFoundException("Блюда с таким id не существует");
		   return _mapper.Map<DishDetailsDTO>(dish);
        }

		public async Task<DishesPagedListDTO> GetDishes(DishFilterModelDTO model, Guid restarauntId) {
			return await GetExistingDishes(model, restarauntId,false);
		}
		public async Task<DishesPagedListDTO> GetExistingDishes(DishFilterModelDTO model, Guid restarauntId, bool deleted = false) {
			var rest = await _context.Restaraunts
				.Include(r => r.Menus)
				.ThenInclude(m => m.Dishes)
				.FirstOrDefaultAsync(x => x.Id == restarauntId);
			if (!model.MenusId.IsNullOrEmpty()) {
				rest.Menus = rest.Menus.Where(m => model.MenusId.Contains(m.Id)).ToList();
			}
			if (rest == null) throw new KeyNotFoundException("Меню с таким id не найдено");
			var dishes = new List<Dish>();

			dishes = rest.Menus
				   .Where(m=> !m.DeletedTime.HasValue)
				   .SelectMany(m => m.Dishes
				   .Where(d => model.Categories
				   .Contains(d.Category) 
				   && (model.Vegetarian ? d.IsVagetarian : true)
				   && (deleted? d.DeletedTime.HasValue : !d.DeletedTime.HasValue)))
				   .ToList();

			var totalItems = dishes.Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / AppConstants.DishPageSize);

			if (totalPages < model.Page && model.Page != 1) throw new BadRequestException("Неверно указана текущая страница");

			var dishDetails = dishes.Select(x => _mapper.Map<DishDetailsDTO>(x)).ToList();
			switch (model.Sorting) {
				case DishSorting.NameAsc:
					dishDetails = dishDetails.OrderBy(x => x.Name).ToList();
					break;
				case DishSorting.NameDesc:
					dishDetails = dishDetails.OrderByDescending(x => x.Name).ToList();
					break;
				case DishSorting.RatingAsc:
					dishDetails = dishDetails.OrderBy(x => x.Rating).ToList();
					break;
				case DishSorting.RatingDesc:
					dishDetails = dishDetails.OrderByDescending(x => x.Rating).ToList();
					break;
				case DishSorting.PriceAsc:
					dishDetails = dishDetails.OrderBy(x => x.Price).ToList();
					dishDetails = dishDetails.OrderBy(x => x.Price).ToList();
					break;
				case DishSorting.PriceDesc:
					dishDetails = dishDetails.OrderByDescending(x => x.Price).ToList();
					break;
			}
			dishDetails = dishDetails
				.Skip((model.Page - 1) * AppConstants.DishPageSize)
				.Take(AppConstants.DishPageSize)
				.ToList();

			_logger.LogInformation("Dishes returned from DishServce");
			return new DishesPagedListDTO {
				Dishes = dishDetails,
				PageInfo = new PageInfoDTO {
					Count = totalPages,
					Current = model.Page,
					Size = AppConstants.DishPageSize
				}
			};
			
		}


		public async Task<Response> RecoverDish(Guid dishId) {

			var dish = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с таким id не найдено");
			dish.DeletedTime = null;
			await _context.SaveChangesAsync();
			return new Response {
				Message = "Succesfully recovered",
				Status = "200"
			};
		}
		
	}
}
