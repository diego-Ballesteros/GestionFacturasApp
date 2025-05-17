using MediatR;

namespace Facturacion.API.Application.Features.Invoices.Queries.GetByID;

public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
{
    public int Id { get; set; }

    public GetInvoiceByIdQuery(int id)
    {
        Id = id;
    }
}
