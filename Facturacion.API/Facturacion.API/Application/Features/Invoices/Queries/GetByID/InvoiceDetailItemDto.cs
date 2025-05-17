﻿namespace Facturacion.API.Application.Features.Invoices.Queries.GetByID;

public class InvoiceDetailItemDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}
