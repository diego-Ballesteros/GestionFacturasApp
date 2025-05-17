using AutoMapper;
using Facturacion.API.Application.Features.Invoices.Commands.Create;
using Facturacion.API.Application.Features.Invoices.Queries.GetAll;
using Facturacion.API.Application.Features.Invoices.Queries.GetByID;
using Facturacion.API.Domain.Entities;

namespace Invoicing.API.Application.Common.Mappings;

public class InvoiceMappingProfile : Profile 
{
    public InvoiceMappingProfile()
    {
        CreateMap<CreateInvoiceCommand, Invoice>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details)); 

        CreateMap<CreateInvoiceDetailDto, InvoiceDetail>();
        CreateMap<Invoice, InvoiceSummaryDto>();
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<InvoiceDetail, InvoiceDetailItemDto>();
    }
}
