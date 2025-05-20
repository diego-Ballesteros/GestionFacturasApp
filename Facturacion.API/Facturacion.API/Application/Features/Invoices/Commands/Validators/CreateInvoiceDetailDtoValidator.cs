using FluentValidation;
namespace Facturacion.API.Application.Features.Invoices.Commands.Create.Validators;

public class CreateInvoiceDetailDtoValidator : AbstractValidator<CreateInvoiceDetailDto>
{
    public CreateInvoiceDetailDtoValidator()
    {
        RuleFor(d => d.ProductName)
               .NotEmpty().WithMessage("Product name is required.")
               .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(d => d.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1.");

        RuleFor(d => d.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
    }
}
