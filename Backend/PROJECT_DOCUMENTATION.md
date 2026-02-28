# IDK 项目文档

概述
- 该仓库包含两个主要项目：`src/IDK`（类库）和 `src/IDK.Server`（示例/宿主应用）。
- 目标框架：.NET 10。

项目结构

- `src/IDK`
  - 主要提供与 MCP（Model Context Protocol）相关的依赖注入扩展、配置和工具定义。
  - 关键文件：
    - `DependencyInjection/ServiceCollectionExtensions.cs`：提供扩展方法 `AddIDKMcp`，用于注册 MCP 服务、工具提供者以及配置 HTTP 传输。
    - `DependencyInjection/McpEndpointRouteBuilderExtensions.cs`：提供 `MapIDKMcp` 扩展，映射 MCP 路由（调用底层 `MapMcp`）。
    - `Configuration/IDKMcpOptions.cs`：容器的配置类（当前为空，可扩展）。
    - `Configuration/IIDKMcpBuilder.cs`：`IIDKMcpBuilder` 接口与实现 `IDKMcpBuilder`，用于访问 `IMcpServerBuilder`。
    - `Tools/IToolDefinitionProvider.cs`：定义 `IToolDefinitionProvider` 接口及默认实现 `ToolDefinitionProvider`。
    - `Tools/ToolDefinition.cs`：描述工具的序列化模型（`Tool`、端口、路径等）。

- `src/IDK.Server`
  - 示例宿主程序，展示如何在 `WebApplication` 中使用库。
  - `Program.cs` 示例：
    - 使用 `builder.Services.AddIDKMcp(...)` 注册 MCP，并通过 `.McpServerBuilder.WithTools<ClockTool>()` 注册工具。
    - 将 MCP 路由映射到 `mcp` 路径：`app.MapIDKMcp("mcp")`。

主要概念与使用

- 注册与配置
  - 在 `Startup` 或 `Program.cs` 中调用：

    builder.Services.AddIDKMcp(options => {
        // 配置 IDKMcpOptions
    }, mcpOptions => {
        // 可选：配置 McpServerOptions
    }, httpTransportOptions => {
        // 可选：配置 HTTP 传输选项
    })
    .McpServerBuilder // 访问底层 Mcp 配置
    .WithTools<YourTool>() // 注册工具（示例中使用 ClockTool）

- 路由映射
  - 在 `WebApplication` 上调用 `app.MapIDKMcp("mcp")` 将 MCP 端点映射到指定路径。

- 工具提供者
  - `IToolDefinitionProvider` 负责列出工具与调用工具。默认实现 `ToolDefinitionProvider` 从构造时注入的一组 `McpServerTool` 构建内部集合并提供调用/列出行为。
  - `ToolDefinition` 是工具的序列化元数据（例如名称、端口和路径）。

运行与构建

- 本地构建：
  - 需要 .NET 10 SDK。
  - 在仓库根目录运行：

    dotnet build

- 运行示例服务器：

    cd src/IDK.Server
    dotnet run --project IDK.Server.csproj

扩展点与备注

- `IDKMcpOptions` 目前为空，适合按需添加配置项。
- `ToolDefinitionProvider.ExecuteToolAsync` 在工具未找到时返回一个错误结果（代码中有 TODO 提示，可根据需求改善错误处理与日志）。
- 若需添加新的工具：实现对应的 `McpServerTool`，并在构建链上通过 `.WithTools<T>()` 或在 DI 容器中注册 `McpServerTool` 实例集合以便 `ToolDefinitionProvider` 注入。

常见任务

- 添加配置项：在 `IDKMcpOptions` 中添加属性，并在 `AddIDKMcp` 调用时通过 `services.Configure` 配置。
- 自定义工具列表：实现 `IToolDefinitionProvider` 并用 `services.AddSingleton<IToolDefinitionProvider, YourProvider>()` 覆盖默认实现。

联系方式与后续

- 若需更详细的 API 文档（类型/方法签名、XML 注释），建议使用 `dotnet doc`／docfx 或在代码中添加 XML 注释并生成文档站点。

- 本文档为仓库当前实现的概览；如需把文档添加到 `docs/` 下或生成更细粒度的引用文档，告诉我目标格式（Markdown、HTML、PDF）及范围（全部 public API 或仅使用说明），我会生成追加内容。