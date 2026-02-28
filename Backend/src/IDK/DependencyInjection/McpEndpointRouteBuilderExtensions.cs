using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace IDK.DependencyInjection;

public static class McpEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapIDKMcp(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "")
    {
        return endpoints.MapMcp(pattern);
    }
}