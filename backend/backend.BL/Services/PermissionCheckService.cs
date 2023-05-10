using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DAL.Data;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.BL.Services {
	public class PermissionCheckService : IPermissionCheckService {
		private readonly BackendDbContext _context;
		public PermissionCheckService(BackendDbContext context) {
			_context = context;
		}
		public async Task CheckPermissionForCook(Guid orderId, Guid cookId) {
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (order.CookerId != cookId) {
				throw new NotAllowedException("Этот заказ не принадлжит этому повару");
			}
		}

		public async Task CheckPermissionForCourier(Guid orderId, Guid courierId) {
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			if (order.CourId != courierId) {
				throw new NotAllowedException("Этот заказ не принадлжит этому курьеру");
			}
		}

		public async Task CheckPermissionForCustomer(Guid orderId, Guid customerId) {
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
			if (order == null) throw new KeyNotFoundException("заказа с таким id не найдено");
			var customer = await _context.Customers.Include(o => o.Orders).FirstOrDefaultAsync(o => o.Id == customerId);
			if (customer == null) throw new KeyNotFoundException("пользователя с таким id не найдено");
			if (!customer.Orders.Contains(order)) throw new NotAllowedException("Этот заказ не принадлежит этому пользователю");
		}

		public async Task CheckPermissionForManagerByMenu(Guid menuId, Guid managerId) {
			var menu = await _context.Menus.Include(m=>m.Restaraunt).ThenInclude(r=>r.Managers).FirstOrDefaultAsync(m => m.Id == menuId);
			if (menu == null) throw new KeyNotFoundException("меню с таким id не найдено");
			if (menu.Restaraunt.Managers == null) throw new KeyNotFoundException("в ресторане, в котором содержится меню, нет менеджеров");
			if (!menu.Restaraunt.Managers.Any(x => x.Id == managerId)) throw new NotFoundException("этот менеджер не имеет отношения к этому меню");
		}

		public async Task CheckPermissionForManagerByRestaraunt(Guid restarauntId, Guid managerId) {
			var restaraunt = await _context.Restaraunts.Include(x => x.Managers).FirstOrDefaultAsync(x => x.Id == restarauntId);
			if (restaraunt == null) throw new KeyNotFoundException("ресторана с таким id не найдено");
			if (restaraunt.Managers == null) throw new KeyNotFoundException("у этого ресторана нет менеджеров");
			if (!restaraunt.Managers.Any(x => x.Id == managerId)) throw new NotFoundException("этот менеджер не имеет отношения к этому ресторану");
		}

	
	}
}
