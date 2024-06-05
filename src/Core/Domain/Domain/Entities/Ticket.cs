
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public string Pnr { get; set; }
        public string TicketNumber { get; set; }
        public int Status { get; set; }
        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

    }
}
