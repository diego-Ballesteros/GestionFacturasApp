using Facturacion.API.Application.Features.Invoices.Commands.Create;
using Facturacion.API.Application.Features.Invoices.Queries.GetByID;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Facturacion.API.Application.Features.Invoices.Commands.Update;

public class UpdateInvoiceCommand : IRequest<InvoiceDto?>
{
    [JsonIgnore]
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; }

    public List<CreateInvoiceDetailDto> Details { get; set; } = new List<CreateInvoiceDetailDto>();

}
