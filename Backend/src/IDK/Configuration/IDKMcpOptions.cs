using ModelContextProtocol.Server;

namespace IDK.Configuration;

public class IDKMcpOptions
{
    public McpServerPrimitiveCollection<McpServerTool> Tools { get; set; }
    public McpServerResourceCollection Resources { get; set; }
    public McpServerPrimitiveCollection<McpServerPrompt> Prompts { get; set; }
}
