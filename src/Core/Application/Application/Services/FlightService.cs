using Application.Interfaces.Repository;
using AutoMapper;
using Core.Application.Dtos;
using Core.Application.Interfaces.Services;

namespace Core.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository flightRepository;
        private readonly IMapper mapper;

        public FlightService(IFlightRepository flightRepository, IMapper mapper)
        {
            this.flightRepository = flightRepository;
            this.mapper = mapper;
        }

        public async Task<List<FlightDto>> GetFlightOptions(string departure, string destination, DateTime date)
        {
            var flights = await flightRepository.GetAllAsync(x => x.Departure == departure && x.Destination == destination);
            if (flights != null)
                return mapper.Map<List<FlightDto>>(flights);
            else
                return null;
        }

    }
}
