using Facturacion.API.Application.Features.Invoices.Commands.Create;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 

namespace Facturacion.API.Controllers;

[Route("api/[controller]")]
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

    [HttpGet("{id}", Name = "GetInvoiceById")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoiceById(int id)
    {
        await Task.CompletedTask;
        return Ok(new { Message = $"Placeholder for GetInvoiceById with ID: {id}. To be implemented." });
    }
}
