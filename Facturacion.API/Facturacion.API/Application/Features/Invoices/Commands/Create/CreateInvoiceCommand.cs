using Facturacion.API.Application.Common.Results;
using MediatR;

namespace Facturacion.API.Application.Features.Invoices.Commands.Create;

public class CreateInvoiceCommand : IRequest<Result<int>>
{
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public List<CreateInvoiceDetailDto> Details { get; set; } = new List<CreateInvoiceDetailDto>();
}
