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
using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.BL.Services {
	public class BasketService : IBasketService {
		private readonly ILogger<BasketService> _logger;
		private readonly BackendDbContext _context;
		private readonly IMapper _mapper;
		public BasketService(ILogger<BasketService> logger, BackendDbContext context, IMapper mapper) {
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}
		public async Task<Response> AddDishToBasket(Guid dishId, Guid customerId) {

			var dish = await _context.Dishes.Include(d => d.Menus).ThenInclude(m => m.Restaraunt).FirstOrDefaultAsync(x => x.Id == dishId);
			if (dish == null) throw new KeyNotFoundException("блюда с таким id не найдено");
			if (dish.DeletedTime.HasValue) throw new NotAllowedException("блюдо удалено");

			var customer = await _context.Customers
				.Include(x=>x.DishInCart)
				.ThenInclude(d=>d.Dish)
				.ThenInclude(d=>d.Menus)
				.ThenInclude(m=>m.Restaraunt)
				.FirstOrDefaultAsync(x=>x.Id == customerId);
			if (customer == null) {
				customer = new Customer() { Id = customerId };
				customer.DishInCart.Add(new DishInCart {
					Count = 1,
					Customer = customer,
					Dish = dish,
				});
				await _context.Customers.AddAsync(customer);
				await _context.SaveChangesAsync();
				return new Response {
					Status = "200",
					Message = "succesfully added"
				};
			}
			
			if (customer.DishInCart.Any(d => d.Dish.Menus.FirstOrDefault().Restaraunt.Id != dish.Menus.FirstOrDefault().Restaraunt.Id))
			throw new NotAllowedException("В корзине содержатся блюда из другого ресторана, очистите ее");

			if (!customer.DishInCart.Any(d => d.Dish.Id == dish.Id)) {
				await _context.AddAsync(new DishInCart {
					Count = 1,
					Customer = customer,
					Dish = dish,
				});
			}
			else {
				customer.DishInCart.FirstOrDefault(d => d.Dish.Id == dish.Id).Count += 1;
				
			}
			await _context.SaveChangesAsync();
			return new Response {
				Status = "200",
				Message = "succesfully added"
			};
				
		}

		public async Task<string> CheckBasketOnDishesFromOneRestaraunt(Guid customerId) {
			var customer = await _context.Customers
				.Include(x => x.DishInCart)
				.ThenInclude(d => d.Dish)
				.ThenInclude(d => d.Menus)
				.ThenInclude(m => m.Restaraunt)
				.FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null)	throw new KeyNotFoundException("пользователь не найден");

			var restaurantName = customer
				.DishInCart
				.FirstOrDefault()?
				.Dish?.Menus?
				.FirstOrDefault()?
				.Restaraunt!.Name;
			return restaurantName ?? "Корзина пользователя пуста";
		}

		public async Task<Response> ClearBasket(Guid customerId) {
			var customer = await _context.Customers
				.Include(x => x.DishInCart)
				.ThenInclude(d => d.Dish)
				.FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null) throw new KeyNotFoundException("пользователь не найден");
			customer.DishInCart.Clear();
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = "basket succesfully cleared"
			};
		}

		public async Task<BasketDTO> GetBasket(Guid customerId) {
			var customer = await _context.Customers
				.Include(x => x.DishInCart)
				.ThenInclude(d => d.Dish)
				.FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null) {
				customer = new Customer() { Id = customerId };
				await _context.Customers.AddAsync(customer);
				await _context.SaveChangesAsync();
			}
				//if (customer == null) throw new KeyNotFoundException("пользователь не найден");
			var dishes = customer.DishInCart.Select(x=> _mapper.Map<DishShortModelDTO>(x)).ToList();
			return new BasketDTO {
				Dishes = customer.DishInCart.Select(x => _mapper.Map<DishShortModelDTO>(x)).ToList(),
				BasketPrice = dishes.Select(x => x.TotalPrice).Sum()
			};

		}

		public async Task<Response> RemoveDish(Guid dishId, Guid customerId, bool CompletelyDelete) {
			var customer = await _context.Customers
				.Include(x => x.DishInCart)
				.ThenInclude(d => d.Dish)
				.FirstOrDefaultAsync(x => x.Id == customerId);
			if (customer == null) throw new KeyNotFoundException("пользователь не найден");
			var dishCart = customer.DishInCart.FirstOrDefault(x => x.Id == dishId);
			if (dishCart == null) throw new KeyNotFoundException("такого блюда нет в корзине пользователя");
			if (CompletelyDelete | dishCart.Count == 1) {
				customer.DishInCart.Remove(dishCart);
			}
			else {
				dishCart.Count -= 1;
			}
			await _context.SaveChangesAsync();

			return new Response {
				Status = "200",
				Message = "succesfully removed"
			};
		}
	}
}
