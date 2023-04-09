using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.BL.Data;
using Auth.BL.Data.Entities;
using Backend.DAL.Data;
using Common.AdmipPanelInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AdmipPanel.BL.Services {
    public class AdminPanelService : IAdmpinPanelService {
        private readonly ILogger<AdminPanelService> _logger;
        private readonly BackendDbContext _contextBackend;
        private readonly AuthDbContext _contextAuth;
        public AdminPanelService(ILogger<AdminPanelService> logger, BackendDbContext context, AuthDbContext authDb) {
            _logger = logger;
            _contextBackend = context;
            _contextAuth = authDb;
        }

    }
}
