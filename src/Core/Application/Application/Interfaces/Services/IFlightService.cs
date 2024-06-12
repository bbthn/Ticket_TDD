using Core.Application.Dtos;

namespace Core.Application.Interfaces.Services
{
    public interface IFlightService
    {
        public Task<List<FlightDto>> GetFlightOptions(string departure, string destination, DateTime date);
    }
   
}
