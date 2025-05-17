using AutoMapper;
using Facturacion.API.Application.Common.Results;
using Facturacion.API.Domain.Entities;
using Facturacion.API.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.API.Application.Features.Invoices.Queries.GetByID;

public class GetInvoiceByIdHandler : IRequestHandler<GetInvoiceByIdQuery, Result<InvoiceDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceByIdHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<InvoiceDto>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
                .AsNoTracking()
                .Include(i => i.Details)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice == null)
        {
            return Result.Failure<InvoiceDto>(Error.NotFound(nameof(Invoice), request.Id)); ; 
        }
       
        var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

        return Result.Success(invoiceDto);
    }
}
