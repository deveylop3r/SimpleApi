using System.Text.Json.Serialization;

namespace SimpleApi.Models;

public class TicketStatus : IEquatable<TicketStatus>
{
  [JsonPropertyName("ticket_id")]
  public required string TicketId { get; set; }

  [JsonPropertyName("statuscode")]
  public required string StatusCode { get; set; }

  [JsonPropertyName("document_key")]
  public List<string> DocumentKeys { get; set; }

  public bool Equals(TicketStatus? other)
  {
    return other is not null && other.TicketId == TicketId;
  }

  public override string ToString()
  {
    return TicketId + ":" + StatusCode;
  }

  public TicketStatus()
  {
    TicketId = string.Empty;
    StatusCode = string.Empty;
    DocumentKeys = [];
  }
}