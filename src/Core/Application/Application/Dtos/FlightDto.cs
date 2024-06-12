namespace Core.Application.Dtos
{
    public class FlightDto
    {
        public Guid Id { get; set; }    
        public DateTime FlightDate { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
    }
}
