using Facturacion.API.Application.Common.Results;
using MediatR;

namespace Facturacion.API.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceCommand : IRequest<Result>
{
    public int Id { get; set; }

    public DeleteInvoiceCommand(int id)
    {
        Id = id;
    }
}
