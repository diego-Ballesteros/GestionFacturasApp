using Facturacion.API.Application.Common.Results;
using MediatR;

namespace Facturacion.API.Application.Features.Invoices.Queries.GetAll;

public class GetAllInvoicesQuery : IRequest<Result<List<InvoiceSummaryDto>>>
{
}
