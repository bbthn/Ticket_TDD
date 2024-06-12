using AutoMapper;
using Core.Application.Models;
using Core.Domain.Entities.Identity;

namespace Core.Application.Mappins
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequest, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
