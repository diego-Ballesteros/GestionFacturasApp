namespace Facturacion.API.Application.Features.Invoices.Queries.GetByID;

public class InvoiceDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<InvoiceDetailItemDto> Details { get; set; } = new List<InvoiceDetailItemDto>();

}
