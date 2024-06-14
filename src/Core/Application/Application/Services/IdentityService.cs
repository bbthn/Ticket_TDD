using AutoMapper;
using Core.Application.Defaults;
using Core.Application.Dtos;
using Core.Application.Interfaces.Services;
using Core.Application.Models;
using Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        public IdentityService(UserManager<AppUser> userManager, IMapper mapper, SignInManager<AppUser> signInManager, ITokenService tokenService, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateCustomer(CreateUserRequest req)
        {
            AppUser user = _mapper.Map<AppUser>(req);
            IdentityResult res = await _userManager.CreateAsync(user, req.Password);
            if (res.Succeeded)
            {
                var roleRes = await _userManager.AddToRoleAsync(user, IdentityDefaults.CustomerRole);
                if(roleRes.Succeeded)
                    return true;
                else
                    return false;
            }
            return false;
            
        }
        public async Task<bool> CreateAdmin(CreateUserRequest req)
        {
            AppUser adminUser = _mapper.Map<AppUser>(req);

            IdentityResult user = await _userManager.CreateAsync(adminUser, req.Password);
            if (user.Succeeded)
            {
                var roleRes = await _userManager.AddClaimAsync(adminUser, new Claim(IdentityDefaults.AdminClaimType,IdentityDefaults.AdminClaimValue));
                if (roleRes.Succeeded)
                    return true;
                else
                    return false;
            }
            return false;

        }

        public async Task<AccessTokenDto> Login(LoginRequest req)
        {

            AppUser user = await _userManager.FindByNameAsync(req.Username);
            if (user != null)
            {
                SignInResult signRes = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);
                if (signRes.Succeeded)
                {
                    AccessTokenDto accessTokenDto =  await _tokenService.CreateAccessToken(user);
                    if (accessTokenDto != null)
                    {
                        await UpdateRefreshToken(accessTokenDto.RefreshToken, user, accessTokenDto.Expiration); // refreshToken' ı dbye yazıyor.
                        return accessTokenDto;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;

        }
        public async Task<bool> CreateRole(string roleName)
        {
            var role = new AppRole(roleName);
            var res = await _roleManager.CreateAsync(role);
            if (res.Succeeded)
                return true;
            return false;
        }

        private async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accesTokenDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accesTokenDate.AddMinutes(5); 
            var res = await _userManager.UpdateAsync(user);
        }

        public async Task<AccessTokenDto> RefreshTokenLogin(string req)
        {

            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == req && u.RefreshTokenEndDate > DateTime.Now);
            if (user != null)
            {
                AccessTokenDto accessTokenDto = await _tokenService.CreateAccessToken(user);
                if (accessTokenDto != null)
                {
                    await UpdateRefreshToken(accessTokenDto.RefreshToken, user, accessTokenDto.Expiration);
                    return accessTokenDto;
                }
                else
                    return null;
            }
            else
                return null;
           
        }
    }
}
