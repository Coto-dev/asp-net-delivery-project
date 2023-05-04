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
using Common.Exceptions;
using CoomonThings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.BL.Services {
	public class MenuService : IMenuService {
		private readonly ILogger<MenuService> _logger;
		private readonly BackendDbContext _context;
		private readonly IMapper _mapper;
		public MenuService(ILogger<MenuService> logger, BackendDbContext context, IMapper mapper) {
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}

		public async Task<Response> AddDishToMenu(Guid menuId, Guid dishId) {
			var menu = await _context.Menus.Include(x=>x.Dishes).FirstOrDefaultAsync(x=>x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");
			var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с таким id не существует");
			menu.Dishes.Add(dish);
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "succesfully added"
			};
		}

		public async Task<Response> CreateMenu(Guid restarauntId, MenuShortModelDTO model) {
			if (model.Name == null) throw new ArgumentNullException(nameof(model.Name));
			var rest = await _context.Restaraunts.Include(r=>r.Menus).FirstOrDefaultAsync(x => x.Id == restarauntId);
			if (rest == null) throw new KeyNotFoundException("Ресторан с таким id не найден");
			if (model.Name == "<<hidden>>") throw new NotAllowedException("Использовано служебное название меню");
			
			await _context.AddAsync(new Menu
			{ Name = model.Name,
			 Restaraunt = rest 
			});
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "succesfully added"
			};
		}

		public async Task<Response> DeleteDishFromMenu(Guid menuId, Guid dishId) {
			var menu = await _context.Menus.Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");
			var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с таким id не существует");
			if (!menu.Dishes.Contains(dish)) throw new BadRequestException("Это блюдо не относиться к этому меню");
			menu.Dishes.Remove(dish);
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "succesfully deleted"
			};
		}

		public async Task<Response> DeleteMenu(Guid menuId) {
			var menu = await _context.Menus.Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");
			if (menu.Name == "<<hidden>>") throw new NotAllowedException("Невозможно удалить служебное меню");
			menu.DeletedTime= DateTime.Now;
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = "succesfully soft deleted"
			};
		}

		public async Task<Response> EditMenu(Guid menuId, MenuShortModelDTO model) {
			var menu = await _context.Menus.Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");
			if (model.Name == "<<hidden>>") throw new NotAllowedException("Невозможно изменить меню,т.к использовано служебное название");
			menu.Name = model.Name;
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = "succesfully edited"
			};
		}

		public async Task<List<MenuDTO>> GetDeletedMenus(Guid restarauntId) {
			var rest = await _context.Restaraunts.Include(r => r.Menus).FirstOrDefaultAsync(x=>x.Id == restarauntId);
			if (rest == null) throw new KeyNotFoundException("Ресторан с таким id не найден");
			return rest.Menus.Where(m=>m.DeletedTime.HasValue).Select(m=> _mapper.Map<MenuDTO>(m)).ToList();
		}

		public async Task<MenuDishesPagedListDTO> GetMenuDetails(Guid menuId, int Page =1) {
			var menu = await _context.Menus.Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");

			if (Page <= 0 || Page == null) throw new BadRequestException("Неверно указана страница");

			var totalItems = await _context.Restaraunts.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / AppConstants.PageSize);

			if (totalPages < Page) throw new BadRequestException("Неверно указана текущая страница");
			menu.Dishes = menu.Dishes
				   .Skip((Page - 1) * AppConstants.PageSize)
				   .Take(AppConstants.PageSize)
				   .ToList();
			var response = new MenuDishesPagedListDTO {
				Menu = _mapper.Map<MenuDTO>(menu),
				PageInfo = new PageInfoDTO {
					Count = totalPages,
					Current = Page,
					Size = AppConstants.PageSize
				}
			};
			return response;
		}

		public async Task<List<MenuShortDTO>> GetMenus(Guid restarauntId) {
			var rest = await _context.Restaraunts.Include(x=>x.Menus).FirstOrDefaultAsync(x => x.Id == restarauntId);
			if (rest == null) throw new KeyNotFoundException("Ресторан с таким id не найден");
			return rest.Menus.Where(m => !m.DeletedTime.HasValue).Select(m => _mapper.Map<MenuShortDTO>(m)).ToList();
		}

		public async Task<Response> RecoverMenu(Guid menuId) {
			var menu = await _context.Menus.FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");
			menu.DeletedTime = null;
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "succesfully recovered"
			};
		}
	}
}
