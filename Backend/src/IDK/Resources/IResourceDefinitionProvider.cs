using IDK.Configuration;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace IDK.Resources;

public interface IResourceDefinitionProvider
{
    ValueTask<ListResourcesResult> ListResourcesAsync(RequestContext<ListResourcesRequestParams> context, CancellationToken cancellationToken = default);
    ValueTask<ListResourceTemplatesResult> ListResourceTemplatesAsync(RequestContext<ListResourceTemplatesRequestParams> context, CancellationToken cancellationToken = default);
    ValueTask<ReadResourceResult> ReadResourceAsync(RequestContext<ReadResourceRequestParams> context, CancellationToken cancellationToken = default);
}


public class ResourceDefinitionProvider : IResourceDefinitionProvider
{
    private readonly McpServerResourceCollection resources;

    public ResourceDefinitionProvider(IDKMcpOptions options)
    {
        this.resources = options.Resources ?? new McpServerResourceCollection();
    }

    public async ValueTask<ListResourcesResult> ListResourcesAsync(RequestContext<ListResourcesRequestParams> context, CancellationToken cancellationToken = default)
    {
        return new ListResourcesResult()
        {
            Resources = resources.Select(r => r.ProtocolResource).ToList(),
        };
    }

    public async ValueTask<ListResourceTemplatesResult> ListResourceTemplatesAsync(RequestContext<ListResourceTemplatesRequestParams> context, CancellationToken cancellationToken = default)
    {
        return new ListResourceTemplatesResult()
        {

        };
    }

    public async ValueTask<ReadResourceResult> ReadResourceAsync(RequestContext<ReadResourceRequestParams> context, CancellationToken cancellationToken = default)
    {
        if (context.Params?.Uri is { } uri && resources is not null)
        {
            // First try an O(1) lookup by exact match.
            if (resources.TryGetPrimitive(uri, out var resource) && !resource.IsTemplated)
            {
                context.MatchedPrimitive = resource;
            }
            else
            {
                // Fall back to an O(N) lookup, trying to match against each URI template.
                foreach (var resourceTemplate in resources)
                {
                    if (resourceTemplate.IsMatch(uri))
                    {
                        context.MatchedPrimitive = resourceTemplate;
                        break;
                    }
                }
            }
        }

        if (context.MatchedPrimitive is McpServerResource matchedResource)
        {
            return await matchedResource.ReadAsync(context, cancellationToken).ConfigureAwait(false);
        }

        return null;
    }
}