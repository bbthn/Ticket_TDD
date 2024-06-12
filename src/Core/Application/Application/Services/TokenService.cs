using Core.Application.Dtos;
using Core.Application.Interfaces.Services;
using Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }


        public async Task<AccessTokenDto> CreateAccessToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.NameSurname),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var claimRes = await _userManager.GetClaimsAsync(user);
            claims.AddRange(claimRes);
             

            AccessTokenDto tokenDto = new AccessTokenDto();
            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_configuration["Auth:SigningKey"]));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            tokenDto.Expiration = DateTime.UtcNow.AddSeconds(15); // test

            JwtSecurityToken jwtSecurityToken = new(
                signingCredentials: signingCredentials,
                claims: claims,
                audience: _configuration["Auth:Audience"],
                issuer: _configuration["Auth:Issuer"],
                expires: tokenDto.Expiration,
                notBefore: DateTime.UtcNow
                );
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            tokenDto.AccessToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            tokenDto.RefreshToken = CreateRefreshToken();
            return tokenDto;

        }

        private string CreateRefreshToken()
        {
            byte[] number = new byte[32];

            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(number);
            return Convert.ToBase64String(number);

        }
    }
}
