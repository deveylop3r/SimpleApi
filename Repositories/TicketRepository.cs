using SimpleApi.Models;
using System.Text.Json;

namespace SimpleApi.Repositories;

public class TicketRepository : ITicketRepository
{
  private readonly ILogger<TicketRepository> _logger;
  private readonly string _dataFilePath;
  private Dictionary<string, TicketStatusData>? _ticketsCache;
  private Dictionary<string, List<TicketChange>>? _changesCache;

  public TicketRepository(ILogger<TicketRepository> logger, IWebHostEnvironment env)
  {
    _logger = logger;
    _dataFilePath = Path.Combine(env.ContentRootPath, "Data", "tickets-mock.json");
  }

  public async Task<List<TicketStatus>> FetchAllTicketsAsync()
  {
    var tickets = await LoadTicketsAsync();
    return tickets
           .Where(kvp => !kvp.Value.IsDeleted)
           .Select(kvp => new TicketStatus
           {
             TicketId = kvp.Value.TicketId,
             StatusCode = kvp.Value.StatusCode,
             DocumentKeys = kvp.Value.DocumentKeys
           })
           .ToList();
  }
  public async Task<TicketStatusData?> GetTicketStatusAsync(string ticketId)
  {
    var tickets = await LoadTicketsAsync();
    return tickets.TryGetValue(ticketId, out var ticket) ? ticket : null;
  }

  public async Task<List<TicketChange>> GetTicketChangesAsync(string ticketId, DateTime lastRequestDate)
  {
    var changes = await LoadChangesAsync();

    if (changes.TryGetValue(ticketId, out var ticketChanges))
    {
      return ticketChanges.Where(c => c.ChangeDate > lastRequestDate).ToList();
    }

    return new List<TicketChange>();
  }

  public async Task<List<TicketStatus>> GetStatusesAsync(IEnumerable<string> ticketIds)
  {
    var tickets = await LoadTicketsAsync();

    return ticketIds
        .Where(id => tickets.ContainsKey(id))
        .Select(id => new TicketStatus
        {
          TicketId = id,
          StatusCode = tickets[id].StatusCode,
          DocumentKeys = tickets[id].DocumentKeys
        })
        .ToList();
  }

  private async Task<Dictionary<string, TicketStatusData>> LoadTicketsAsync()
  {
    if (_ticketsCache != null)
      return _ticketsCache;

    try
    {
      var json = await File.ReadAllTextAsync(_dataFilePath);
      var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
      var data = JsonSerializer.Deserialize<TicketMockData>(json, options);

      _ticketsCache = data?.Tickets ?? new Dictionary<string, TicketStatusData>();
      return _ticketsCache;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error loading tickets mock data");
      return new Dictionary<string, TicketStatusData>();
    }
  }

  private async Task<Dictionary<string, List<TicketChange>>> LoadChangesAsync()
  {
    if (_changesCache != null)
      return _changesCache;

    try
    {
      var json = await File.ReadAllTextAsync(_dataFilePath);
      var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
      var data = JsonSerializer.Deserialize<TicketMockData>(json, options);

      _changesCache = data?.Changes ?? new Dictionary<string, List<TicketChange>>();
      return _changesCache;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error loading changes mock data");
      return new Dictionary<string, List<TicketChange>>();
    }
  }

  private class TicketMockData
  {
    public Dictionary<string, TicketStatusData> Tickets { get; set; } = new();
    public Dictionary<string, List<TicketChange>> Changes { get; set; } = new();
  }
}