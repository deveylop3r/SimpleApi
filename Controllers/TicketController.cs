using Microsoft.AspNetCore.Mvc;
using SimpleApi.Models;
using SimpleApi.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SimpleApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
  private readonly ITicketRepository _repository;
  private readonly ILogger<TicketController> _logger;

  public TicketController(ITicketRepository repository, ILogger<TicketController> logger)
  {
    _repository = repository;
    _logger = logger;
  }

  [HttpGet("Ping")]
  public string Ping()
  {
    return "Pong";
  }

  [HttpGet("all")]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> FetchAllTickets()
  {
    try
    {
      _logger.LogInformation("Fetching all active tickets");

      var tickets = await _repository.FetchAllTicketsAsync();

      _logger.LogInformation("Successfully retrieved {TicketCount} tickets", tickets.Count);

      return Ok(tickets);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching all tickets");
      return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving tickets");
    }
  }

  [HttpPost("submitTicketData")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult SubmitTicketData([FromBody] SubmitTicketDataContract data)
  {

    if (data.Names.Count() != data.Values.Count())
    {
      return BadRequest("The number of names must match the number of values.");
    }

    return StatusCode(StatusCodes.Status201Created,
      new { message = "Ticket data submitted successfully", data });
  }

  [HttpPost("submitTicketAttachment")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public IActionResult SubmitTicketAttachment([FromBody] SubmitTicketAttachmentContract attachment)
  {
    var newFileName = attachment.FileName.ToLowerInvariant();

    var result = new SubmitTicketAttachmentResult
    {
      TicketId = attachment.TicketId,
      FileName = newFileName,
      DocumentKey = Guid.NewGuid().ToString("D")
    };

    return StatusCode(StatusCodes.Status201Created, result);
  }

  [HttpGet("getTicketAttachment")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult GetTicketAttachment([FromQuery] string ticketId, [FromQuery] string documentKey)
  {
    if (string.IsNullOrWhiteSpace(ticketId) || string.IsNullOrWhiteSpace(documentKey))
    {
      return BadRequest("Both ticketId and documentKey are required.");
    }

    var fakeFileContent = "This is the document content";
    var documentContentBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(fakeFileContent));

    var result = new GetTicketAttachmentResult
    {
      TicketId = ticketId,
      DocumentKey = documentKey,
      DocumentName = "document.pdf",
      DocumentContent = documentContentBase64
    };
    return Ok(result);
  }

  [HttpPost("statusRequestBatch")]
  [ProducesResponseType(typeof(IEnumerable<TicketStatus>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> StatusRequestBatch([FromBody] StatusRequestBatchContract data)
  {
    var ticketStatuses = await _repository.GetStatusesAsync(data.TicketIds);
    return Ok(ticketStatuses);
  }

  [HttpGet("getChangedTicketData")]
  [ProducesResponseType(typeof(ChangedTicketStatusResult), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Ticket not found or deleted")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetChangedTicketData([FromQuery][Required][StringLength(50, MinimumLength = 1)] string ticketId)
  {
    var ticketStatus = await _repository.GetTicketStatusAsync(ticketId);

    if (ticketStatus == null || ticketStatus.IsDeleted)
    {
      return NoContent();
    }

    var ticketChanges = await _repository.GetTicketChangesAsync(ticketId, ticketStatus.LastRequestDate);

    var result = new ChangedTicketStatusResult
    {
      TicketId = ticketId,
      StatusCode = ticketStatus.StatusCode,
      DocumentKeys = ticketStatus.DocumentKeys,
      TicketChanges = ticketChanges
    };

    return Ok(result);
  }
}