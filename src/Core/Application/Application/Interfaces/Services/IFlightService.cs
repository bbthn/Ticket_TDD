using Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Services
{
    public interface IFlightService
    {
        public Task<List<FlightDto>> GetFlightOptions(string departure, string destination, DateTime date);
    }
   
}
