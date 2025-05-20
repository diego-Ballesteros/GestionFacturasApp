using Facturacion.API.Application.Features.Invoices.Commands.Delete;
using FluentValidation;

namespace Facturacion.API.Application.Features.Invoices.Commands.Validators;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0).WithMessage("Invoice ID must be greater than 0.");
    }
}
