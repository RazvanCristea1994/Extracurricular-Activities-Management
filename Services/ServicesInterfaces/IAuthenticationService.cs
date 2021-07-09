using ExtracurricularActivitiesManagement.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services.ServicesInterfaces
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>> RegisterUser(RegisterRequest registerRequest);
        Task<bool> ConfirmUserRequest(ConfirmUserRequest confirmUserRequest);
        Task<ServiceResponse<LoginResponse, string>> LoginUser(LoginRequest loginRequest);
        Task<ServiceResponse<AuthenticationUserResponse, string>> GetCurrentUser(string value);
    }
}
