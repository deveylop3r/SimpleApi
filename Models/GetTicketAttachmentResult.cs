using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SimpleApi.Models;

public class GetTicketAttachmentResult
{
  [JsonPropertyName("ticket_id")]
  [StringLength(9, MinimumLength = 8)]
  public required string TicketId { get; set; }

  [JsonPropertyName("document_key")]
  public required string DocumentKey { get; set; }

  [JsonPropertyName("document_name")]
  public required string DocumentName { get; set; }

  [JsonPropertyName("document_content")]
  public required string DocumentContent { get; set; }

  public override string ToString()
  {
    return $"{nameof(TicketId)}:{TicketId}|{nameof(DocumentKey)}:{DocumentKey}|{nameof(DocumentName)}:{DocumentName}";
  }

}