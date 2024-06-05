using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Application.Dtos;
using Core.Domain.Entities;

namespace Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }
        public async Task<TicketDto> GetTicketAsync(string pnr)
        {
            var ticket = await _ticketRepository.GetSingleAsync(x => x.Pnr == pnr);
            if (ticket != null)
                return _mapper.Map<TicketDto>(ticket);
            else
                return null;

        }
        public async Task<TicketDto> VoidTicketAsync(TicketDto ticket)
        {
            ticket.Status = 0;
            var res = await _ticketRepository.UpdateAsync(_mapper.Map<Ticket>(ticket));
            if(res != null)
                return _mapper.Map<TicketDto>(res);
            return null;
            
        }
        public bool ValidateTicket(string pnr, string ticketNumber)
        {
            return pnr.Equals(ticketNumber);
        }

        public async Task<TicketDto> ReissueTicket(TicketDto ticket , Guid newFlightId)
        {
            ticket.FlightId = newFlightId;
            var updatedTicket = await _ticketRepository.UpdateAsync(_mapper.Map<Ticket>(ticket));
            if (updatedTicket != null)
                return _mapper.Map<TicketDto>(updatedTicket);
            else
                return null;                      
        }
    }
    
}
