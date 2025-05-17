using AutoMapper;
using Facturacion.API.Application.Features.Invoices.Queries.GetByID;
using Facturacion.API.Domain.Entities;
using Facturacion.API.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.API.Application.Features.Invoices.Commands.Update;

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, InvoiceDto?>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateInvoiceCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDto?> Handle(UpdateInvoiceCommand command, CancellationToken cancellationToken)
    {
        var invoiceToUpdate = await _context.Invoices
            .Include(i => i.Details) 
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (invoiceToUpdate == null)
        {
            return null; 
        }

        invoiceToUpdate.CustomerName = command.CustomerName;
        invoiceToUpdate.InvoiceDate = command.InvoiceDate;

        var updatedDetailEntities = _mapper.Map<List<InvoiceDetail>>(command.Details);

        var detailsToRemove = invoiceToUpdate.Details
            .Where(existingDetail => !updatedDetailEntities.Any(updatedDetail => updatedDetail.ProductName == existingDetail.ProductName && updatedDetail.UnitPrice == existingDetail.UnitPrice && updatedDetail.Quantity == existingDetail.Quantity)) // Simplificación: se podría usar un ID de detalle si viniera del frontend
            .ToList();

        foreach (var detailToRemove in detailsToRemove)
        {
            _context.InvoiceDetails.Remove(detailToRemove);
        }

        invoiceToUpdate.Details.Clear(); 

        decimal calculatedTotalAmount = 0;
        foreach (var detailDto in command.Details) 
        {
            var newDetailEntity = _mapper.Map<InvoiceDetail>(detailDto); 
            newDetailEntity.Subtotal = newDetailEntity.Quantity * newDetailEntity.UnitPrice; 

            invoiceToUpdate.Details.Add(newDetailEntity); 
            calculatedTotalAmount += newDetailEntity.Subtotal;
        }

        invoiceToUpdate.TotalAmount = calculatedTotalAmount; 

       
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {          
            if (!await _context.Invoices.AnyAsync(i => i.Id == command.Id, cancellationToken))
            {
                return null; 
            }
            else
            {
                throw; 
            }
        }

        var updatedInvoiceDto = _mapper.Map<InvoiceDto>(invoiceToUpdate);
        return updatedInvoiceDto;
    }
}
