using ModelContextProtocol.Protocol;
using System.Text.Json.Serialization;

namespace IDK.Tools;

public class ToolDefinition
{
    [JsonPropertyName("tool")]
    public required Tool Tool { get; set; }

    [JsonIgnore]
    public string Name => Tool.Name;

    [JsonPropertyName("port")]
    public int Port { get; set; } = 443;

    [JsonPropertyName("path")]
    public string Path { get; set; } = "/score";
}
