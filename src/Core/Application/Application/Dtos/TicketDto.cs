namespace Core.Application.Dtos
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Pnr { get; set; }
        public string TicketNumber { get; set; }
        public int Status { get; set; }
        public Guid FlightId { get; set; }
    }
}
