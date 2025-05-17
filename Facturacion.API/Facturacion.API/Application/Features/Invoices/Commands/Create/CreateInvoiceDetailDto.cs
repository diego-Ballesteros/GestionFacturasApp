namespace Facturacion.API.Application.Features.Invoices.Commands.Create;

public class CreateInvoiceDetailDto
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
