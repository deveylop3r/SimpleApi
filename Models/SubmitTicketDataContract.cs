using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class SubmitTicketDataContract
{
    [Required(ErrorMessage = "Field names are required")]
    [MinLength(1, ErrorMessage = "At least one field name is required")]
    [JsonPropertyName("names")]
    public required IEnumerable<string> Names { get; set; }

    [Required(ErrorMessage = "Field values are required")]
    [MinLength(1, ErrorMessage = "At least one field value is required")]
    [JsonPropertyName("values")]
    public required IEnumerable<string> Values { get; set; }
}