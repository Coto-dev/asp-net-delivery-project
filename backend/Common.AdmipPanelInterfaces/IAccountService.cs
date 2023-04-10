using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace Common.AdmipPanelInterfaces {
    public interface IAccountService {
        public Task Login(LoginCredentials model);
        public Task Logout();

    }
}
