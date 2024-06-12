using Core.Application.Dtos;
using Core.Application.Models;

namespace Core.Application.Interfaces.Services
{
    public interface IIdentityService
    {

        public Task<bool> CreateCustomer(CreateUserRequest req);
        public Task<bool> CreateAdmin(CreateUserRequest req);
        public Task<bool> CreateRole(string roleName);

        public Task<AccessTokenDto> Login(LoginRequest req);
        public Task<AccessTokenDto> RefreshTokenLogin(string req);

    }
}
