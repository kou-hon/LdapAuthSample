
using LdapAuthSample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZLogger;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<LdapOptions>(builder.Configuration.GetSection(nameof(LdapOptions)));

builder.Services.AddLogging(logging =>
{
	logging.AddZLoggerConsole();
	// logging.ClearProviders(); は .NET 8 では不要
});

builder.Services.AddSingleton<LdapAuthServiceSimple>();
builder.Services.AddSingleton<LdapAuthServiceAdvanced>();
builder.Services.AddTransient<App>();

var host = builder.Build();

var app = host.Services.GetRequiredService<App>();
await app.RunAsync();

