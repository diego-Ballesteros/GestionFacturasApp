using Facturacion.API.Application.Features.Invoices.Commands.Create;
using Facturacion.API.Application.Features.Invoices.Commands.Delete;
using Facturacion.API.Application.Features.Invoices.Commands.Update;
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

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]   
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceCommand command)
    {
        if (id != command.Id && command.Id != 0) 
        {                                        
            if (command.Id == 0) command.Id = id;
            else return BadRequest(new { Message = "Route ID does not match command ID." });
        }
        else if (command.Id == 0) 
        {
            command.Id = id;
        }
        
        try
        {
            var updatedInvoiceDto = await _mediator.Send(command);

            if (updatedInvoiceDto == null)
            {
                return NotFound(new { Message = $"Invoice with ID {id} not found for update." });
            }

            return Ok(updatedInvoiceDto);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred while updating the invoice.", Details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]   
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        var command = new DeleteInvoiceCommand(id);

        try
        {
            var result = await _mediator.Send(command);

            if (!result) 
            {
                return NotFound(new { Message = $"Invoice with ID {id} not found." });
            }

            return NoContent(); 
        }
        catch (FluentValidation.ValidationException ex) 
        {
            return BadRequest(new { Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred while deleting the invoice.", Details = ex.Message });
        }
    }
}
