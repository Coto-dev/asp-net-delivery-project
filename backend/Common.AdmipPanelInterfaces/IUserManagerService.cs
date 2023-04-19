using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace Common.AdminPanelInterfaces {
	public interface IUserManagerService {
		public Task<List<UsersViewModel>> GetUsers(string? Email);
		public Task EditUser (UsersViewModel model);
		public Task BanUser (Guid id);
	}
}
