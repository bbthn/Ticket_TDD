using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Extensions.Auth
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection ConfiureAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer("user", opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = configuration["Auth:Audience"],
                    ValidIssuer = configuration["Auth:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:SigningKey"])),
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false
                };
            });
            return services;          
        }

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("basicUser", opt =>
                {
                    opt.RequireRole("customer");
                });


                opt.AddPolicy("admin", opt =>
                {
                    opt.RequireClaim("whoareyou", "admin", "developer");
                });

            });
            return services;
        }
    }
}
