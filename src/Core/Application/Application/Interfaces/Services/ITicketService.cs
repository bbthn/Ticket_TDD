
using Application.Interfaces.Repository;
using Application.Services;
using Core.Application.Dtos;

namespace Application.Interfaces.Services
{
    public interface ITicketService
    {
        public Task<TicketDto> VoidTicketAsync(TicketDto ticket);
        public Task<TicketDto> GetTicketAsync(string pnr);
        public bool ValidateTicket(string ticketNumber1, string ticketNumber2);
        public Task<TicketDto> ReissueTicket(TicketDto ticket, Guid newFlightId);


    }
}
