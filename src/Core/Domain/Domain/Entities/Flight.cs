namespace Core.Domain.Entities
{
    public class Flight : BaseEntity
    {
        public DateTime FlightDate { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
