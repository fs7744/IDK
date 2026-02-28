using ModelContextProtocol.Server;
using System.ComponentModel;

namespace IDK.Server.Demo;


[McpServerResourceType]
public class VersionResource
{
    [McpServerResource, Description("IDK version resource ")]
    public static string IDKVersionResource() => typeof(VersionResource).Assembly.GetName().Version.ToString();
}
