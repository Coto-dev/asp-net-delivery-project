using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using AutoMapper;
using Backend.DAL.Data;
using Common.AdminPanelInterfaces;
using Common.DTO;
using Common.Enums;
using CookRest = Backend.DAL.Data.Entities.Cook;
using ManagerRest = Backend.DAL.Data.Entities.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdmipPanel.BL.Services {
	public class UserManagerService:IUserManagerService {
		private readonly ILogger<UserManagerService> _logger;
		private readonly BackendDbContext _contextBackend;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public UserManagerService(ILogger<UserManagerService> logger, BackendDbContext context,
			UserManager<User> userManager, IMapper mapper) {
			_logger = logger;
			_contextBackend = context;
			_userManager = userManager;
			_mapper = mapper;
		}

		public async Task BanUser(Guid id) {
			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
			await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(10));
		}

		public async Task EditUser(UsersViewModel model) {
			var user = await _userManager.Users.Include(u => u.Customer).Include(u=>u.Manager).Include(u => u.Courier).Include(u => u.Cook).FirstOrDefaultAsync(u => u.Id == model.Id);
			if (user == null) throw new KeyNotFoundException("User not found");

			user.FullName = Regex.Replace(model.FullName, @"\s+", " ");
			user.BirthDate = model.BirthDate;
			user.Gender = model.Gender;
			user.PhoneNumber = model.PhoneNumber;
			if (model.Roles != null) {
				var userRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, userRoles);
				await _userManager.AddToRolesAsync(user, model.Roles);
				model.Roles.ForEach(role => {
					if (role == ApplicationRoleNames.Cook) user.Cook = new Cook { User = user };
					if (role == ApplicationRoleNames.Courier) user.Courier = new Courier { User = user  };
					if (role == ApplicationRoleNames.Manager) user.Manager = new Manager { User = user };
					if (role == ApplicationRoleNames.Customer) user.Customer = new Customer { User = user,Address = model.Address};
				});
				
				if (!model.Roles.Contains(ApplicationRoleNames.Cook) && userRoles.Contains(ApplicationRoleNames.Cook)) {
					var rest = await _contextBackend.Restaraunts.Include(r => r.Cooks).Where(r => r.Cooks.Contains(new CookRest { Id = model.Id })).FirstOrDefaultAsync();
					if (rest != null) {
						rest.Cooks.Remove(rest.Cooks.FirstOrDefault(r => r.Id == model.Id));
						await _contextBackend.SaveChangesAsync();
					}
				}
				if (!model.Roles.Contains(ApplicationRoleNames.Manager) && userRoles.Contains(ApplicationRoleNames.Manager)) {
					var rest = await _contextBackend.Restaraunts.Include(r => r.Managers).Where(r => r.Managers.Contains(new ManagerRest { Id = model.Id })).FirstOrDefaultAsync();
					if (rest != null) {
						rest.Managers.Remove(rest.Managers.FirstOrDefault(r => r.Id == model.Id));
						await _contextBackend.SaveChangesAsync();
					}
				}
			}
			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded) {
				var errors = string.Join(", ", result.Errors.Select(x => x.Description));
				throw new InvalidOperationException(errors);
			}

		}

		public async Task<List<UsersViewModel>> GetUsers(string? Email) {
			
			if (Email != null)
				return await _userManager.Users.Include(r=>r.Roles).ThenInclude(r => r.Role).Include(c => c.Customer).Where(u=>u.Email.Contains(Email)).Select(u=> _mapper.Map<UsersViewModel>(u)).ToListAsync();
			return await _userManager.Users.Include(r => r.Roles).ThenInclude(r => r.Role).Include(c=>c.Customer).Select(u => _mapper.Map<UsersViewModel>(u)).Take(20).ToListAsync();
		}
	}
}
