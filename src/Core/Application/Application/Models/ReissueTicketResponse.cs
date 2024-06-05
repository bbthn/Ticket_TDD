using Core.Application.Dtos;

namespace Core.Application.Models
{
    public class ReissueTicketResponse
    {
        public List<FlightDto> Flights { get; set; }
        public TicketDto Ticket { get; set; }

    }
}
