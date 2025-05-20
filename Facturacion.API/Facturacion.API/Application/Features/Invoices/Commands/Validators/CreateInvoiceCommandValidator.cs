using FluentValidation;

namespace Facturacion.API.Application.Features.Invoices.Commands.Create.Validators;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {

        RuleFor(c => c.CustomerName)
                    .NotEmpty().WithMessage("Customer name is required.")
                    .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

        RuleFor(c => c.InvoiceDate)
                    .NotEmpty().WithMessage("Invoice date is required.")
                    .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Invoice date cannot be in the future.");

        RuleFor(c => c.Details)
                    .NotEmpty().WithMessage("At least one invoice detail is required.");

        RuleForEach(c => c.Details)
                    .SetValidator(new CreateInvoiceDetailDtoValidator());

    }
}
