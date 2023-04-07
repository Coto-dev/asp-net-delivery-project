using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace Common.AuthInterfaces {
    public interface IAccountService {
        Task<AuthenticatedResponse> Register(RegisterModelDTO registerModel);
        Task<AuthenticatedResponse> Login(LoginCredentials login);
        Task<Response> EditUserToCustomer(string address, string userName);
        Task<ProfileDTO> GetProfile(string userName);
        Task<Response> EditProfile(EditProfileDTO model, string userName);
        Task<AuthenticatedResponse> Refresh(TokenApiModel tokenApiModel);
        Task<Response> Logout(string userName);
    }
}
