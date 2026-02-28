using IDK.Configuration;
using IDK.Prompts;
using IDK.Resources;
using IDK.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Server;

namespace IDK.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IIDKMcpBuilder AddIDKMcp(this IServiceCollection services, Action<IDKMcpOptions> configureOptions = null, Action<McpServerOptions> configureMcpOptions = null, Action<HttpServerTransportOptions>? configureHttpServerTransportOptions = null)
    {
        var opt = new IDKMcpOptions();
        services.AddSingleton(opt);
        services.TryAddSingleton<IToolDefinitionProvider, ToolDefinitionProvider>();
        services.TryAddSingleton<IResourceDefinitionProvider, ResourceDefinitionProvider>();
        services.TryAddSingleton<IPromptsDefinitionProvider, PromptsDefinitionProvider>();

        return new IDKMcpBuilder(services.AddMcpServer(options =>
        {
            configureMcpOptions?.Invoke(options);
            opt.Tools = options.ToolCollection ?? new McpServerPrimitiveCollection<McpServerTool>();
            opt.Resources = options.ResourceCollection ?? new McpServerResourceCollection();
            opt.Prompts = options.PromptCollection ?? new McpServerPrimitiveCollection<McpServerPrompt>();
            options.ToolCollection = null;
            options.ResourceCollection = null;
            options.PromptCollection = null;
        })
            .WithListToolsHandler(static (c, ct) =>
            {
                var provider = c.Services!.GetRequiredService<IToolDefinitionProvider>();
                return provider?.ListToolsAsync(c, ct) ?? throw new InvalidOperationException("Tool registry not properly registered.");
            })
            .WithCallToolHandler(static (c, ct) =>
            {
                var provider = c.Services!.GetRequiredService<IToolDefinitionProvider>();
                return provider?.ExecuteToolAsync(c, ct) ?? throw new InvalidOperationException("Tool registry not properly registered.");
            })
            .WithListResourcesHandler(static (c, ct) =>
            {
                var provider = c.Services!.GetRequiredService<IResourceDefinitionProvider>();
                return provider?.ListResourcesAsync(c, ct) ?? throw new InvalidOperationException("Resource registry not properly registered.");
            })
            .WithReadResourceHandler(static (c, ct) =>
            {
                var provider = c.Services!.GetRequiredService<IResourceDefinitionProvider>();
                return provider?.ReadResourceAsync(c, ct) ?? throw new InvalidOperationException("Resource registry not properly registered.");
            })
            .WithListResourceTemplatesHandler(static (c, ct) =>
            {
                var provider = c.Services!.GetRequiredService<IResourceDefinitionProvider>();
                return provider?.ListResourceTemplatesAsync(c, ct) ?? throw new InvalidOperationException("Resource registry not properly registered.");
            })
            .WithListPromptsHandler(static (c, ct) =>
            {
                var provider = c.Services!.GetRequiredService<IPromptsDefinitionProvider>();
                return provider?.ListPromptsAsync(c, ct) ?? throw new InvalidOperationException("Prompts registry not properly registered.");
            })
            .WithGetPromptHandler(static (c, ct) =>
            {
                var provider = c.Services!.GetRequiredService<IPromptsDefinitionProvider>();
                return provider?.GetPromptAsync(c, ct) ?? throw new InvalidOperationException("Prompts registry not properly registered.");
            })
            .WithHttpTransport(options =>
            {
                configureHttpServerTransportOptions?.Invoke(options);
            })
        );
    }
}
