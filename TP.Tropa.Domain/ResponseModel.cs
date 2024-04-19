using System.Text.Json.Serialization;

namespace TP.Tropa.Domain;

public class ResponseModel
{
    public int Status { get; set; }
    public string? Description { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Stacktrace { get; set; }
}
