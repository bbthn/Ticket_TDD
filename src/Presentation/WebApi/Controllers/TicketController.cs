using Application.Interfaces.Services;
using Core.Application.Dtos;
using Core.Application.Interfaces.Services;
using Core.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "user")]
    [EnableRateLimiting("Basic")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IFlightService _flightService;
        private readonly ILogger _logger;

        public TicketController(ITicketService ticketService, IFlightService flightService,ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _flightService = flightService;
            _logger = logger;
        }

        [HttpGet]
        [Route("echo/{req:maxlength(10)}")]
        [Authorize(Policy = "basicUser")]
        public async Task<IActionResult> Alive(string req)
        {
            _logger.LogInformation(req);
            return Ok($"{req}");
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

            _logger.LogInformation($"Ticket {ticket.TicketNumber} was voided");
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

            _logger.LogInformation($"Ticket {ticket.TicketNumber} was reissued");
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

        [HttpGet("addticket")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> AddTicket()
        {
            return Ok("Ticket Eklendi!");
        }
    }

 

   
}
