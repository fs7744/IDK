using IDK.Configuration;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace IDK.Tools;

public interface IToolDefinitionProvider
{
    ValueTask<CallToolResult> ExecuteToolAsync(RequestContext<CallToolRequestParams> context, CancellationToken cancellationToken = default);
    ValueTask<ListToolsResult> ListToolsAsync(RequestContext<ListToolsRequestParams> context, CancellationToken cancellationToken = default);
}

public class ToolDefinitionProvider : IToolDefinitionProvider
{
    private readonly McpServerPrimitiveCollection<McpServerTool> tools;

    public ToolDefinitionProvider(IDKMcpOptions options)
    {
        this.tools = options.Tools ?? new();
    }

    public async ValueTask<CallToolResult> ExecuteToolAsync(RequestContext<CallToolRequestParams> context, CancellationToken cancellationToken = default)
    {
        if (context.Params?.Name is { } toolName && tools is not null &&
                   tools.TryGetPrimitive(toolName, out var tool) && tool is not null)
        {
            context.MatchedPrimitive = tool;
            return await tool.InvokeAsync(context, cancellationToken).ConfigureAwait(false);
        }

        //todo 
        return new()
        {
            IsError = true,
            Content = [new TextContentBlock() { Text = $"Tool '{context.Params?.Name}' not found." }]
        };
    }

    public async ValueTask<ListToolsResult> ListToolsAsync(RequestContext<ListToolsRequestParams> context, CancellationToken cancellationToken = default)
    {
        //todo 
        return new ListToolsResult
        {
            Tools = tools.Select(i => i.ProtocolTool).ToList(),
            NextCursor = null
        };
    }
}