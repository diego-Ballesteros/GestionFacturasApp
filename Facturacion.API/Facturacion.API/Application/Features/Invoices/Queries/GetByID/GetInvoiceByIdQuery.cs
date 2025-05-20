using Facturacion.API.Application.Common.Results;
using MediatR;

namespace Facturacion.API.Application.Features.Invoices.Queries.GetByID;

public class GetInvoiceByIdQuery : IRequest<Result<InvoiceDto>>
{
    public int Id { get; set; }

    public GetInvoiceByIdQuery(int id)
    {
        Id = id;
    }
}
