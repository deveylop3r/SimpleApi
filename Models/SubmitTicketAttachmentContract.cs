using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleApi.Models;

public class SubmitTicketAttachmentContract
{
  [Required(ErrorMessage = "Ticket ID is required")]
  [StringLength(20, MinimumLength = 1, ErrorMessage = "Ticket ID must be between 1 and 20 characters")]
  [JsonPropertyName("ticket_id")]
  public required string TicketId { get; set; }

  [Required(ErrorMessage = "File content is required")]
  [MinLength(1, ErrorMessage = "File content cannot be empty")]
  [JsonPropertyName("bytes")]
  public required byte[] Bytes { get; set; }

  [Required(ErrorMessage = "File name is required")]
  [StringLength(255, MinimumLength = 1, ErrorMessage = "File name must be between 1 and 255 characters")]
  [JsonPropertyName("file_name")]
  public required string FileName { get; set; }
}