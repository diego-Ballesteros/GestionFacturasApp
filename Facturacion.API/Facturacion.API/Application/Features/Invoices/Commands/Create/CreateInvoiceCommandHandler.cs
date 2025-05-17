
using MediatR; 
using Facturacion.API.Domain.Entities; 
using Facturacion.API.Infrastructure.Persistence.Data;
using AutoMapper;
using Facturacion.API.Application.Common.Results;

namespace Facturacion.API.Application.Features.Invoices.Commands.Create;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateInvoiceCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {

        var invoiceEntity = _mapper.Map<Invoice>(command);

        decimal calculatedTotalAmount = 0;

        foreach (var detailEntity in invoiceEntity.Details)
        {
            detailEntity.Subtotal = detailEntity.Quantity * detailEntity.UnitPrice;

            calculatedTotalAmount += detailEntity.Subtotal;
        }

        invoiceEntity.TotalAmount = calculatedTotalAmount;

        throw new InvalidOperationException("¡Boom! Error de prueba inesperado desde el handler.");

        _context.Invoices.Add(invoiceEntity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(invoiceEntity.Id);
    }
}
