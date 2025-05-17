using Facturacion.API.Application.Features.Invoices.Commands.Create;
using Facturacion.API.Application.Features.Invoices.Commands.Delete;
using Facturacion.API.Application.Features.Invoices.Commands.Update;
using Facturacion.API.Application.Features.Invoices.Queries.GetAll;
using Facturacion.API.Application.Features.Invoices.Queries.GetByID;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Facturacion.API.Application.Common.Results;

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

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, new { InvoiceId = result.Value }) 
            : HandleFailure (result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<InvoiceSummaryDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<InvoiceSummaryDto>>> GetAllInvoices()
    {
        var result = await _mediator.Send(new GetAllInvoicesQuery());

        return Ok(result.Value);
    }

    [HttpGet("{id}", Name = "GetInvoiceById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInvoiceById(int id)
    {
        var result = await _mediator.Send(new GetInvoiceByIdQuery(id));

        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
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

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.Value) 
            : HandleFailure(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]   
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        var result = await _mediator.Send(new DeleteInvoiceCommand(id));

        return result.IsSuccess
            ? NoContent() 
            : HandleFailure(result);
    }
    private IActionResult HandleFailure(Result result)
    {
        if (result.Error == Error.NullValue) 
        {
            return BadRequest(result.Error);
        }
        if (result.Error.Code.Contains("NotFound"))
        {
            return NotFound(result.Error);
        }
       
        return BadRequest(new { ErrorCode = result.Error.Code, ErrorMessage = result.Error.Message });
    }
}
