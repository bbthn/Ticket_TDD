namespace Core.Application.Models
{
    public class ReissueTicketRequest
    {
        public string Pnr { get; set; }
        public string TicketNumber { get; set; }
        public string NewDeparture { get; set; }
        public string NewDestination { get; set; }
        public DateTime DateTime { get; set; }

    }
}
