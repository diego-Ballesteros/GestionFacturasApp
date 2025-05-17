using Facturacion.API.Application.Features.Invoices.Commands.Create;
using Facturacion.API.Application.Features.Invoices.Queries.GetAll;
using Facturacion.API.Application.Features.Invoices.Queries.GetByID;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 

namespace Facturacion.API.Controllers;

[Route("api/invoices")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator; 

    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // POST: api/invoices
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command)
    {

        try
        {
            var invoiceId = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new { InvoiceId = invoiceId });
        }
        catch (ValidationException ex) 
        {
            return BadRequest(new { Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
        }
        catch (System.Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred. Please try again later.", Details = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<InvoiceSummaryDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<InvoiceSummaryDto>>> GetAllInvoices()
    {
        try
        {
            var query = new GetAllInvoicesQuery(); 
            var invoices = await _mediator.Send(query); 
            return Ok(invoices); 
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred while retrieving invoices.", Details = ex.Message });
        }
    }

    [HttpGet("{id}", Name = "GetInvoiceById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InvoiceDto>> GetInvoiceById(int id)
    {
        try
        {
            var query = new GetInvoiceByIdQuery(id);
            var invoice = await _mediator.Send(query);

            if (invoice == null)
            {
                return NotFound(new { Message = $"Invoice with ID {id} not found." });
            }

            return Ok(invoice);
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred while retrieving the invoice.", Details = ex.Message });
        }
    }
}
