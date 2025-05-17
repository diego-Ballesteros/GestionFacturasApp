using Facturacion.API.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.API.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteInvoiceCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    async Task<bool> IRequestHandler<DeleteInvoiceCommand, bool>.Handle(DeleteInvoiceCommand command, CancellationToken cancellationToken)
    {
        var invoiceToDelete = await _context.Invoices
                                    .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (invoiceToDelete == null)
        {
            return false; 
        }
       
        _context.Invoices.Remove(invoiceToDelete);
        var result = await _context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}
