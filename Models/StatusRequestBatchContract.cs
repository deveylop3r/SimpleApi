using System.ComponentModel.DataAnnotations;

namespace SimpleApi.Models;

public class StatusRequestBatchContract
{
  [Required(ErrorMessage = "Ticket IDs are required")]
  [MinLength(1, ErrorMessage = "At least one ticket ID is required")]
  public IEnumerable<string> TicketIds { get; set; } = [];
}