using Facturacion.API.Application.Common.Results;
using Facturacion.API.Domain.Entities;
using Facturacion.API.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.API.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public DeleteInvoiceCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    async Task<Result> IRequestHandler<DeleteInvoiceCommand, Result>.Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoiceToDelete = await _context.Invoices
                                    .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoiceToDelete == null)
        {
            return Result.Failure(Error.NotFound(nameof(Invoice), request.Id));
        }

        _context.Invoices.Remove(invoiceToDelete);
        var result = await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
