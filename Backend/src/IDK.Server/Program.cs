using IDK.DependencyInjection;
using IDK.Server.Demo;
using IDK.Server.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIDKMcp(c =>
{

}).McpServerBuilder.WithTools<ClockTool>().WithResources<VersionResource>();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseAuthorization();

//app.MapControllers();

app.MapIDKMcp("mcp");

app.Run();
