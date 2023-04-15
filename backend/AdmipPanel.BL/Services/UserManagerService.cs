using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using AutoMapper;
using Backend.DAL.Data;
using Common.AdminPanelInterfaces;
using Microsoft.AspNetCore.Identity;
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

	}
}
