using Application.Interfaces.Services;
using Core.Application.Interfaces.Services;
using Core.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IFlightService _flightService;

        public TicketController(ITicketService ticketService, IFlightService flightService)
        {
            _ticketService = ticketService;
            _flightService = flightService;
        }

        [HttpPost("void")]
        public async Task<IActionResult> VoidTicketAsync([FromBody] VoidTicketRequest req)
        {
            var ticket = await _ticketService.GetTicketAsync(req.Pnr);
            if (ticket == null)
                return NotFound("Ticket is not found");

            if(!_ticketService.ValidateTicket(ticket.TicketNumber, req.TicketNumber))
                return BadRequest("Ticket and Pnr is not matching");

            var voidedTicket = await _ticketService.VoidTicketAsync(ticket);
            if (voidedTicket == null)
                return BadRequest("Ticket is not voided");
            return Ok("Ticket voided successfully");
        }

        [HttpPost("reissue")]
        public async Task<IActionResult> ReissueTicketAsync([FromBody]ReissueTicketRequest req)
        {
            var ticket = await _ticketService.GetTicketAsync(req.Pnr);
            if (ticket == null)
                return NotFound("Ticket is not found");
            if (!_ticketService.ValidateTicket(ticket.TicketNumber, req.TicketNumber))
                return BadRequest("Ticket and Pnr is not matching");

            var flightOptions = await _flightService.GetFlightOptions(req.NewDeparture, req.NewDestination, req.DateTime);
            if(flightOptions == null)
                return NotFound("Flight options are not found");
            return Ok(new ReissueTicketResponse() { Flights=flightOptions,Ticket=ticket});


        }


        [HttpPost("reissue/confirm")]
        public async Task<IActionResult> ReissueConfirmAsync([FromBody]ReissueConfirmRequest req)
        {
            var confirmed = await _ticketService.ReissueTicket(req.Ticket, req.NewFlightId);
            if (confirmed == null)
                return NotFound("Ticket is not found");
            return Ok(confirmed);
           
        }
    }

 

   
}
