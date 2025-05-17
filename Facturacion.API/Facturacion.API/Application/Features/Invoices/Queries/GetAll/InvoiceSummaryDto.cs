namespace Facturacion.API.Application.Features.Invoices.Queries.GetAll;

public class InvoiceSummaryDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
}
