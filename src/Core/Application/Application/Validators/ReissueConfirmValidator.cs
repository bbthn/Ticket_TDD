using Core.Application.Models;
using FluentValidation;
using System.Data;

namespace Core.Application.Validators
{
    public class ReissueConfirmRequestValidator : AbstractValidator<ReissueConfirmRequest>
    {
        public ReissueConfirmRequestValidator()
        {
            RuleFor(request => request.Ticket).NotNull().WithMessage("Ticket cannot be null.");

            RuleFor(request => request.NewFlightId).NotEmpty().WithMessage("NewFlightId cannot be empty.");

            When(request => request.Ticket != null, () =>
            {
                RuleFor(request => request.Ticket.Pnr)
                    .NotEmpty().WithMessage("Pnr cannot be empty.")
                    .Length(10).WithMessage("Pnr must be 10 characters.");

                RuleFor(request => request.Ticket.TicketNumber)
                    .NotEmpty().WithMessage("TicketNumber cannot be empty.")
                    .Length(10).WithMessage("Pnr must be 10 characters.");

                RuleFor(request => request.Ticket.FlightId).NotEmpty().WithMessage("FlightId cannot be empty.");
            });
        }

    }
}
