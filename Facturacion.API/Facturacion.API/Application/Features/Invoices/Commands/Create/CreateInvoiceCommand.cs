using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Facturacion.API.Application.Features.Invoices.Commands.Create;

public class CreateInvoiceCommand : IRequest<int>
{
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public List<CreateInvoiceDetailDto> Details { get; set; } = new List<CreateInvoiceDetailDto>();
}
