using Core.Application.Models;
using FluentValidation;

public class VoidTicketRequestValidator : AbstractValidator<VoidTicketRequest>
{
    public VoidTicketRequestValidator()
    {
        RuleFor(request => request.Pnr)
            .NotEmpty().WithMessage("Pnr cannot be empty.")
            .Length(10).WithMessage("Pnr must be 10 characters.");

        RuleFor(request => request.TicketNumber)
            .NotEmpty().WithMessage("TicketNumber cannot be empty.")
            .Length(10).WithMessage("TicketNumber must be 10 characters.");
    }
}
