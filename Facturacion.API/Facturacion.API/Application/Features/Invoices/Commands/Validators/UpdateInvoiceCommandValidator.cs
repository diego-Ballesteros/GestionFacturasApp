using Facturacion.API.Application.Features.Invoices.Commands.Create.Validators;
using Facturacion.API.Application.Features.Invoices.Commands.Update;
using FluentValidation;

namespace Facturacion.API.Application.Features.Invoices.Commands.Validators;

public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
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
