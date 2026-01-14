using System.Text.Json.Serialization;

namespace SimpleApi.Models;

public class ChangedTicketStatusResult
{
  [JsonPropertyName("ticket_id")]
  public required string TicketId { get; set; }

  [JsonPropertyName("status_code")]
  public required string StatusCode { get; set; }

  [JsonPropertyName("document_keys")]
  public List<string> DocumentKeys { get; set; } = [];

  [JsonPropertyName("ticket_changes")]
  public List<TicketChange> TicketChanges { get; set; } = [];
}

public class TicketChange
{
  [JsonPropertyName("change_id")]
  public required string ChangeId { get; set; }

  [JsonPropertyName("change_type")]
  public required string ChangeType { get; set; } // ADDED, MODIFIED, DELETED

  [JsonPropertyName("change_date")]
  public required DateTime ChangeDate { get; set; }

  [JsonPropertyName("document_name")]
  public required string DocumentName { get; set; }
}