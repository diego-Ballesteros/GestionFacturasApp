using MediatR;

namespace Facturacion.API.Application.Features.Invoices.Queries.GetAll;

public class GetAllInvoicesQuery : IRequest<List<InvoiceSummaryDto>>
{
}
