using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using Auth.DAL.Data;
using Backend.DAL.Data;
using Common.AdminPanelInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Backend.DAL.Data.Entities;
using AutoMapper;
using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using Common.Enums;
using System.Net;
using CookAuth = Auth.DAL.Data.Entities.Cook;
using ManagerAuth = Auth.DAL.Data.Entities.Manager;
using Manager = Backend.DAL.Data.Entities.Manager;
using Cook = Backend.DAL.Data.Entities.Cook;
using System.Runtime.CompilerServices;

namespace AdmipPanel.BL.Services {
    
    public class CrudService : ICrudService {
        private readonly ILogger<CrudService> _logger;
        private readonly BackendDbContext _contextBackend;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CrudService(ILogger<CrudService> logger, BackendDbContext context,
            UserManager<User> userManager, IMapper mapper) {
            _logger = logger;
            _contextBackend = context;
            _userManager = userManager;
            _mapper = mapper;
        }

		public async Task AddCook(string email, Guid restarauntId) {
            var user = await _userManager.Users.Include(u=>u.Cook).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) {
                throw new KeyNotFoundException("user with this email does not exist");
            }
			
               var restraunt = await _contextBackend.Restaraunts.Include(r=>r.Cooks).FirstOrDefaultAsync(r=>r.Id == restarauntId);
			if (restraunt == null) throw new KeyNotFoundException("restaraunt with this id does not exist");

			

			var result = await _userManager.AddToRoleAsync(user, ApplicationRoleNames.Cook);
			var errors = string.Join(", ", result.Errors.Select(x => x.Description));
			if (result.Succeeded || errors == "User already in role 'Cook'.") {
				user.Cook = new CookAuth() {
					Id = user.Id,
				};
				var cook = new Cook() {
					Id = user.Id,
					Restaraunt = restraunt,

				};
				if (restraunt.Cooks.Any(x => x.Id == cook.Id)) throw new ArgumentException("this cook already exist in this restaraunt");
				if (_contextBackend.Restaraunts.Any(x => x.Cooks.Contains(cook))) throw new ArgumentException("this cook already exist in other restaraunt");
				await _contextBackend.AddAsync(cook);
				await _contextBackend.SaveChangesAsync();
				await _userManager.UpdateAsync(user);
			}
			else {
					throw new InvalidOperationException(errors);
			}
		}

		public async Task AddManager(string email, Guid restarauntId) {
			var user = await _userManager.Users.Include(u => u.Manager).FirstOrDefaultAsync(u => u.Email == email);
			if (user == null) {
				throw new KeyNotFoundException("user with this email does not exist");
			}
			var result = await _userManager.AddToRoleAsync(user, ApplicationRoleNames.Manager);
			var errors = string.Join(", ", result.Errors.Select(x => x.Description));
			if (result.Succeeded || errors == "User already in role 'Manager'.") {
				user.Manager = new ManagerAuth() {
					Id = user.Id,
				};
				var restraunt = await _contextBackend.Restaraunts.Include(r => r.Managers).FirstOrDefaultAsync(r => r.Id == restarauntId);
				if (restraunt == null) throw new KeyNotFoundException("restaraunt with this id does not exist");
				var manager = new Manager() {
					Id = user.Id,
					Restaraunt = restraunt,

				};
				if (restraunt.Managers.Any(x => x.Id == manager.Id)) throw new ArgumentException("this manager already exist in this restaraunt");
				if (_contextBackend.Restaraunts.Any(x=>x.Managers.Contains(manager))) throw new ArgumentException("this manager already exist in other restaraunt");
				await _contextBackend.AddAsync(manager);
				await _contextBackend.SaveChangesAsync();
				await _userManager.UpdateAsync(user);
			}
			else {
				throw new InvalidOperationException(errors);
			}
		}

		public async Task CreateRestaraunt(RestarauntViewModel model) {
			var sameRest = await _contextBackend.Restaraunts.FirstOrDefaultAsync(x=>x.Name== model.Name);
			if (sameRest != null) {
				if (!sameRest.DeletedTime.HasValue)
				throw new ArgumentException($"restaraunt with same title - {model.Name} already exists");
			}

			await _contextBackend.AddAsync(new Restaraunt() {
                Name = model.Name,
                PhotoUrl = model.PhotoUrl,
				Address = model.Address,
				Description = model.Description,
            });
            await _contextBackend.SaveChangesAsync();
        }

		public async Task Delete(Guid id) {
			var rest = await _contextBackend.Restaraunts.FirstOrDefaultAsync(x=>x.Id == id);
			rest.DeletedTime = DateTime.UtcNow;
			await _contextBackend.SaveChangesAsync();
		}

		public async Task Edit(EditRestarauntVIew model) {
			var rest = await _contextBackend.Restaraunts.FirstOrDefaultAsync(x => x.Name == model.Name);
			rest.Address = model.Address;
			rest.Description = model.Description;
			rest.PhotoUrl = model.PhotoUrl;
			rest.Name = model.Name;
			await _contextBackend.SaveChangesAsync();
		}

		public async Task<EditRestarauntVIew> GetForEdit(Guid id) {
			var rest = await _contextBackend.Restaraunts.FirstOrDefaultAsync(x => x.Id == id);
			return new EditRestarauntVIew {
				Address = rest.Address,
				Description = rest.Description,
				Name = rest.Name,
				PhotoUrl = rest.PhotoUrl
			};
		}

		public async Task<RestarauntViewModel> GetDetails(Guid id) {
            var viewModel =  _mapper.Map<RestarauntViewModel>(await _contextBackend.Restaraunts.Include(x=>x.Cooks).Include(x=>x.Managers).FirstOrDefaultAsync(x=>x.Id == id));
			var cookEmails = viewModel.CookEmails.Select(x=> _userManager.Users.FirstOrDefault(u=>u.Id.ToString() == x)).Select(u=>u.Email).ToList();
			var managerEmails = viewModel.ManagerEmails.Select(x => _userManager.Users.FirstOrDefault(u => u.Id.ToString() == x)).Select(u => u.Email).ToList();
			viewModel.ManagerEmails = managerEmails;
			viewModel.CookEmails = cookEmails;
			return viewModel;
        }

		public async Task<ViewRestaraunt> GetRestaraunt(Guid id) {
			var rest = await _contextBackend.Restaraunts.FirstOrDefaultAsync(x => x.Id == id);
			if (rest == null) throw new KeyNotFoundException("This restaraunt does not exist");
			return new ViewRestaraunt {
				Id = rest.Id,
				Name = rest.Name
			};
		}

		public List<RestarauntDTO> GetRestarauntList() {
            var Restaraunts =  _contextBackend.Restaraunts.Select(x=> _mapper.Map<RestarauntDTO>(x)).ToList();
            return Restaraunts;
        }

		public async Task DeleteCook(AddUserViewModel model) {
			var rest = await _contextBackend.Restaraunts.Include(x => x.Cooks).FirstOrDefaultAsync(x => x.Id == model.restarauntId);
			var user = await _userManager.Users.Include(x=>x.Cook).FirstOrDefaultAsync(x=>x.Email == model.Email);
			var cookRestaraunt = rest.Cooks.FirstOrDefault(x => x.Id == user.Id);
			if (cookRestaraunt == null) {
				throw new ArgumentException("user doesn't work at this restaraunt");
			}
			user.Cook = null;
			var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRoleNames.Cook);
			await _userManager.UpdateAsync(user);
			rest.Cooks.Remove(cookRestaraunt);
			await _contextBackend.SaveChangesAsync();
		}

		public async Task DeleteManager(AddUserViewModel model) {
			var rest = await _contextBackend.Restaraunts.Include(x => x.Managers).FirstOrDefaultAsync(x => x.Id == model.restarauntId);
			var user = await _userManager.Users.Include(x => x.Manager).FirstOrDefaultAsync(x => x.Email == model.Email);
			var managerRestaraunt = rest.Managers.FirstOrDefault(x => x.Id == user.Id);
			if (managerRestaraunt == null) {
				throw new ArgumentException("user doesn't work at this restaraunt");
			}
			user.Manager = null;
			var result = await _userManager.RemoveFromRoleAsync(user, ApplicationRoleNames.Manager);
				await  _userManager.UpdateAsync(user);
				rest.Managers.Remove(managerRestaraunt);
				await _contextBackend.SaveChangesAsync();
		}

		public async Task RecoverRest(Guid id) {
			var rest = await _contextBackend.Restaraunts.FirstOrDefaultAsync(x => x.Id == id);
			rest.DeletedTime = null;
			await _contextBackend.SaveChangesAsync();
		}

		
	}
}
