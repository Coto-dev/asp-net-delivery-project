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
			if (result.Succeeded) {
				user.Cook = new CookAuth() {
					Id = user.Id,
				};
				var cook = new Cook() {
					Id = user.Id,
					Restaraunt = restraunt,

				};
				await _contextBackend.AddAsync(cook);
				await _contextBackend.SaveChangesAsync();
				await _userManager.UpdateAsync(user);
			}
			else {
				var errors = string.Join(", ", result.Errors.Select(x => x.Description));
				throw new InvalidOperationException(errors);
			}
		}

		public async Task AddManager(string email, Guid restarauntId) {
			var user = await _userManager.Users.Include(u => u.Manager).FirstOrDefaultAsync(u => u.Email == email);
			if (user == null) {
				throw new KeyNotFoundException("user with this email does not exist");
			}
			var result = await _userManager.AddToRoleAsync(user, ApplicationRoleNames.Manager);
			if (result.Succeeded) {
				user.Manager = new ManagerAuth() {
					Id = user.Id,
				};
				var restraunt = await _contextBackend.Restaraunts.Include(r => r.Cooks).FirstOrDefaultAsync();
				if (restraunt == null) throw new KeyNotFoundException("restaraunt with this id does not exist");
				var manager = new Manager() {
					Id = user.Id,
					Restaraunt = restraunt,

				};
				await _contextBackend.AddAsync(manager);
				await _contextBackend.SaveChangesAsync();
				await _userManager.UpdateAsync(user);
			}
			else {
				var errors = string.Join(", ", result.Errors.Select(x => x.Description));
				throw new InvalidOperationException(errors);
			}
		}

		public async Task CreateRestaraunt(RestarauntViewModel model) {
          await _contextBackend.AddAsync(new Restaraunt() {
                Name = model.Name,
                PhotoUrl = model.PhotoUrl
            });
            await _contextBackend.SaveChangesAsync();
        }

        public async Task<RestarauntViewModel> GetDetails(Guid id) {
            var viewModel =  _mapper.Map<RestarauntViewModel>(await _contextBackend.Restaraunts.Include(x=>x.Cooks).Include(x=>x.Managers).FirstOrDefaultAsync(x=>x.Id == id));
			var cookEmails = viewModel.CookEmails.Select(x=> _userManager.Users.FirstOrDefault(u=>u.Id.ToString() == x)).Select(u=>u.Email).ToList();
			var managerEmails = viewModel.ManagerEmails.Select(x => _userManager.Users.FirstOrDefault(u => u.Id.ToString() == x)).Select(u => u.Email).ToList();
			viewModel.ManagerEmails = managerEmails;
			viewModel.CookEmails = cookEmails;
			return viewModel;
        }

        public List<RestarauntDTO> GetRestarauntList() {
            var Restaraunts =  _contextBackend.Restaraunts.Select(x=> _mapper.Map<RestarauntDTO>(x)).ToList();
            return Restaraunts;
        }
    }
}
