using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
			var menu = await _context.Menus.Include(x=>x.Restaraunt).Include(x=>x.Dishes).FirstOrDefaultAsync(x=>x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");
			var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с таким id не существует");
			if (!menu.Restaraunt.Menus.Any(m => m.Dishes.Contains(dish))) throw new NotAllowedException("это блюдо принадлежит другому ресторану");
			if (menu.Dishes.Contains(dish)) throw new BadRequestException("это блюдо уже состоит в этом меню");
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
			var dish = _context.Dishes
				.Include(d=>d.Menus)
				.FirstOrDefault(d => d.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("Блюдо с таким id не существует");
			if (!menu.Dishes.Contains(dish)) throw new BadRequestException("Это блюдо не относиться к этому меню");
			if (dish.Menus.Count <= 1) dish.DeletedTime = DateTime.Now;
			else menu.Dishes.Remove(dish);
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "succesfully deleted"
			};
		}

		public async Task<Response> DeleteMenu(Guid menuId) {
			var menu = await _context.Menus.Include(x => x.Dishes).FirstOrDefaultAsync(x => x.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("Меню с с таким id не найдено");
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
			menu.Name = model.Name;
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = "succesfully edited"
			};
		}

		public async Task<List<MenuShortDTO>> GetDeletedMenus(Guid restarauntId) {
			var rest = await _context.Restaraunts.Include(r => r.Menus).FirstOrDefaultAsync(x=>x.Id == restarauntId);
			if (rest == null) throw new KeyNotFoundException("Ресторан с таким id не найден");
			return rest.Menus.Where(m => m.DeletedTime.HasValue).Select(m => _mapper.Map<MenuShortDTO>(m)).ToList();
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
