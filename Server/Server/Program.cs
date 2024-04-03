using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Application;
using Server.Persistence;
using Server.Services;

var config = new ConfigurationManager();

IHost host = Host.CreateDefaultBuilder().ConfigureServices(
    services =>
    {
        services.AddSingleton<IApplicationService, ApplicationService>()
                .AddInfrastructure(config)
                .AddApplication();
    }).Build();

var app = host.Services.GetRequiredService<IApplicationService>();

await app.Run();

