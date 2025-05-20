using AutoMapper;
using AutoMapper.QueryableExtensions;
using Facturacion.API.Application.Common.Results;
using Facturacion.API.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.API.Application.Features.Invoices.Queries.GetAll;

public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, Result<List<InvoiceSummaryDto>>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllInvoicesQueryHandler(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result<List<InvoiceSummaryDto>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoiceSummaries = await _context.Invoices
            .AsNoTracking()
            .OrderByDescending(i => i.InvoiceDate)
            .ProjectTo<InvoiceSummaryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result.Success(invoiceSummaries);
    }
}
