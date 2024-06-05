using AutoMapper;
using Core.Application.Dtos;
using Core.Domain.Entities;

namespace Core.Application.Mappins
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<FlightDto, Flight>().ReverseMap();
        }
    }
}
