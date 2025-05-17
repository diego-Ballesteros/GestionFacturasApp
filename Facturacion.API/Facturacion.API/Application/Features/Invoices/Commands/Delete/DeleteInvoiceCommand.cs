using MediatR;

namespace Facturacion.API.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteInvoiceCommand(int id)
    {
        Id = id;
    }
}
