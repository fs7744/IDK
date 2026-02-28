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
    public ValueTask<GetPromptResult> GetPromptAsync(RequestContext<GetPromptRequestParams> context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<ListPromptsResult> ListPromptsAsync(RequestContext<ListPromptsRequestParams> context, CancellationToken cancellationToken = default)
    {
        return new ListPromptsResult()
        {

        };
    }
}