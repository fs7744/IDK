using Microsoft.Extensions.DependencyInjection;

namespace IDK.Configuration;

public interface IIDKMcpBuilder
{
    public IMcpServerBuilder McpServerBuilder { get; }
}


public class IDKMcpBuilder : IIDKMcpBuilder
{
    public IDKMcpBuilder(IMcpServerBuilder mcpServerBuilder)
    {
        McpServerBuilder = mcpServerBuilder;
    }

    public IMcpServerBuilder McpServerBuilder { get; }
}