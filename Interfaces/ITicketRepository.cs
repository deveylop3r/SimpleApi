using SimpleApi.Models;

namespace SimpleApi.Repositories;

public interface ITicketRepository
{
  Task<TicketStatusData?> GetTicketStatusAsync(string ticketId);
  Task<List<TicketChange>> GetTicketChangesAsync(string ticketId, DateTime lastRequestDate);
  Task<List<TicketStatus>> GetStatusesAsync(IEnumerable<string> ticketIds);
  Task<List<TicketStatus>> FetchAllTicketsAsync();
}

public class TicketStatusData
{
  public required string TicketId { get; set; }
  public required string StatusCode { get; set; }
  public bool IsDeleted { get; set; }
  public DateTime LastRequestDate { get; set; }
  public List<string> DocumentKeys { get; set; } = [];
}