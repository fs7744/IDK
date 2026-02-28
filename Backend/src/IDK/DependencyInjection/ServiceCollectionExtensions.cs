using IDK.Configuration;
using IDK.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Server;

namespace IDK.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IIDKMcpBuilder AddIDKMcp(this IServiceCollection services, Action<IDKMcpOptions> configureOptions, Action<McpServerOptions> configureMcpOptions = null, Action<HttpServerTransportOptions>? configureHttpServerTransportOptions = null)
    {
        services.Configure(configureOptions);

        services.TryAddSingleton<IToolDefinitionProvider, ToolDefinitionProvider>();

        return new IDKMcpBuilder(services.AddMcpServer(options =>
        {
            configureMcpOptions?.Invoke(options);
            options.ToolCollection?.Clear();
        })
            .WithListToolsHandler(static (c, ct) =>
            {
                var toolDefinitionProvider = c.Services!.GetRequiredService<IToolDefinitionProvider>();
                return toolDefinitionProvider?.ListToolsAsync(c, ct) ?? throw new InvalidOperationException("Tool registry not properly registered.");
            })
            .WithCallToolHandler(static (c, ct) =>
            {
                var toolDefinitionProvider = c.Services!.GetRequiredService<IToolDefinitionProvider>();
                return toolDefinitionProvider?.ExecuteToolAsync(c, ct) ?? throw new InvalidOperationException("Tool registry not properly registered.");
            })
            .WithHttpTransport(options =>
            {
                configureHttpServerTransportOptions?.Invoke(options);
            })
        );
    }
}
