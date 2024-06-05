using Core.Application.Dtos;

namespace Core.Application.Models
{
    public class ReissueConfirmRequest
    {
        public TicketDto Ticket { get; set; }
        public Guid NewFlightId { get; set; }
    }
}
