using AutoMapper;
using Core.Application.Dtos;
using Core.Domain.Entities;

namespace Core.Application.Mappins
{
    public class TicketProfile : Profile
    {

        public TicketProfile()
        {
            CreateMap<TicketDto, Ticket>().ReverseMap();
        }

    }
}
