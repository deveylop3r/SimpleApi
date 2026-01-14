using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleApi.Models
{
  public class SubmitTicketAttachmentResult
  {
    [JsonPropertyName("ticket_id")]
    [StringLength(9, MinimumLength = 8)]
    public required string TicketId { get; set; }

    [JsonPropertyName("document_key")]
    public required string DocumentKey { get; set; }

    [JsonPropertyName("file_name")]
    public required string FileName { get; set; }
    public override string ToString()
    {
      return $"{nameof(TicketId)}:{TicketId}|{nameof(DocumentKey)}:{DocumentKey}";
    }
  }
}