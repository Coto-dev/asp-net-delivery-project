using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DAL.Data;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Common.Exceptions;
using CoomonThings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Backend.BL.Services {

	public class DishService : IDishService {
        private readonly BackendDbContext _context;
		private readonly IMapper _mapper;
		public DishService(BackendDbContext context, IMapper mapper) {
            _context = context;
			_mapper = mapper;
        }

        public async Task<Response> AddRatingToDish(Guid dishId, double value, Guid userId) {
			var dish = await _context.Dishes
				.Include(x => x.Ratings)
				.ThenInclude(r=>r.Customer)
				.FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с с таким id не найдено");
			if (dish.DeletedTime.HasValue) throw new NotAllowedException("Блюдо удалено");
			if (! await CheckRating(dishId, userId)) throw new NotAllowedException("Пользователь с таким id не может поставить рейтинг");
			dish.Ratings.Add(new Rating { 
				Dish = dish,
				Value= value,
				CustomerId = userId
			});
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
				&& o.Dishes.DishesCart.Contains(dish)
				&& o.Dishes.Customer.Id == userId);
			if (check == null) return false;
			return true;

		}

		public async Task<Response> CreateDishWithMenu(DishModelDTO model, Guid menuId) {
			var menu = await _context.Menus.Include(m=>m.Dishes).FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с таким id не найдено");

			menu.Dishes.Add(_mapper.Map<Dish>(model));
			await _context.SaveChangesAsync();
			return new Response {
				Message = "Succesfully created",
				Status = "200"
			};
		}

		public async Task<Response> CreateDishWithHiddenMenu(DishModelDTO model, Guid restarauntId) {
			var menu = await _context.Menus.Include(m => m.Dishes).FirstOrDefaultAsync(x => x.Name == "<<hidden>>");
			if (menu == null) throw new KeyNotFoundException("Не найдено скрытое меню");

			menu.Dishes.Add(_mapper.Map<Dish>(model));
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
           var dish =  await _context.Dishes.Include(x=>x.Ratings).FirstOrDefaultAsync();
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

			if (model.Page <= 0 || model.Page == null) throw new BadRequestException("Неверно указана страница");

			var totalItems = rest.Menus.Select(x => x.Dishes.Count).Sum();
			var totalPages = (int)Math.Ceiling((double)totalItems / AppConstants.DishPageSize);

			if (totalPages < model.Page) throw new BadRequestException("Неверно указана текущая страница"); ;
			if (deleted) {
				if (model.Vegetarian == true) {
					dishes = rest.Menus.SelectMany(m => m.Dishes
				   .Where(d => model.Categories
				   .Contains(d.Category) && d.IsVagetarian && d.DeletedTime.HasValue))
				   .Skip((model.Page - 1) * AppConstants.DishPageSize)
				   .Take(AppConstants.DishPageSize)
				   .ToList();

				}
				else {
					dishes = rest.Menus.SelectMany(m => m.Dishes
				   .Where(d => model.Categories
				   .Contains(d.Category) && d.DeletedTime.HasValue))
				   .Skip((model.Page - 1) * AppConstants.DishPageSize)
				   .Take(AppConstants.DishPageSize)
				   .ToList();
				}
			}
			else {
				if (model.Vegetarian == true) {
					dishes = rest.Menus.SelectMany(m => m.Dishes
				   .Where(d => model.Categories
				   .Contains(d.Category) && d.IsVagetarian && !d.DeletedTime.HasValue))
				   .Skip((model.Page - 1) * AppConstants.DishPageSize)
				   .Take(AppConstants.DishPageSize)
				   .ToList();

				}
				else {
					dishes = rest.Menus.SelectMany(m => m.Dishes
				   .Where(d => model.Categories
				   .Contains(d.Category) && !d.DeletedTime.HasValue))
				   .Skip((model.Page - 1) * AppConstants.DishPageSize)
				   .Take(AppConstants.DishPageSize)
				   .ToList();
				}
			}
			var dishDetails = dishes.Select(x => _mapper.Map<DishDetailsDTO>(x)).ToList();
			switch (model.Sorting) {
				case DishSorting.NameAsc:
					dishDetails.OrderBy(x => x.Name);
					break;
				case DishSorting.NameDesc:
					dishDetails.OrderByDescending(x => x.Name);
					break;
				case DishSorting.RatingAsc:
					dishDetails.OrderBy(x => x.Rating);
					break;
				case DishSorting.RatingDesc:
					dishDetails.OrderByDescending(x => x.Rating);
					break;
				case DishSorting.PriceAsc:
					dishDetails.OrderBy(x => x.Price);
					break;
				case DishSorting.PriceDesc:
					dishDetails.OrderByDescending(x => x.Price);
					break;
			}

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
