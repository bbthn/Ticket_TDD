using Core.Application.Models;
using FluentValidation;

public class ReissueTicketRequestValidator : AbstractValidator<ReissueTicketRequest>
{
    public ReissueTicketRequestValidator()
    {
        RuleFor(request => request.Pnr)
            .NotEmpty().WithMessage("Pnr cannot be empty.")
            .Length(10).WithMessage("Pnr must be 10 characters.");

        RuleFor(request => request.TicketNumber)
            .NotEmpty().WithMessage("TicketNumber cannot be empty.")
            .Length(10).WithMessage("Pnr must be 10 characters.");

        RuleFor(request => request.NewDeparture)
            .NotEmpty().WithMessage("NewDeparture cannot be empty.")
            .MaximumLength(50).WithMessage("NewDeparture must be  50 characters.");

        RuleFor(request => request.NewDestination)
            .NotEmpty().WithMessage("NewDestination cannot be empty.")
            .MaximumLength(50).WithMessage("NewDestination must be  50 characters.");

    }
}
