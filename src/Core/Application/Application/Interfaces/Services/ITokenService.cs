using Core.Application.Dtos;
using Core.Domain.Entities.Identity;
using System.Security.Claims;

namespace Core.Application.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<AccessTokenDto> CreateAccessToken(AppUser user);

    }
}
