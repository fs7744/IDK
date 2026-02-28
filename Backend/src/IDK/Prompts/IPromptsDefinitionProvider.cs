using IDK.Configuration;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace IDK.Prompts;

public interface IPromptsDefinitionProvider
{
    ValueTask<GetPromptResult> GetPromptAsync(RequestContext<GetPromptRequestParams> context, CancellationToken cancellationToken = default);
    ValueTask<ListPromptsResult> ListPromptsAsync(RequestContext<ListPromptsRequestParams> context, CancellationToken cancellationToken = default);
}


public class PromptsDefinitionProvider : IPromptsDefinitionProvider
{
    private readonly McpServerPrimitiveCollection<McpServerPrompt> prompts;

    public PromptsDefinitionProvider(IDKMcpOptions options)
    {
        this.prompts = options.Prompts ?? new();
    }

    public async ValueTask<GetPromptResult> GetPromptAsync(RequestContext<GetPromptRequestParams> context, CancellationToken cancellationToken = default)
    {
        if (context.Params?.Name is { } promptName && prompts is not null &&
                    prompts.TryGetPrimitive(promptName, out var prompt))
        {
            context.MatchedPrimitive = prompt;
            return await prompt.GetAsync(context, cancellationToken);
        }
        return null;
    }

    public async ValueTask<ListPromptsResult> ListPromptsAsync(RequestContext<ListPromptsRequestParams> context, CancellationToken cancellationToken = default)
    {
        return new ListPromptsResult()
        {
            Prompts = prompts.Select(p => p.ProtocolPrompt).ToList(),
        };
    }
}