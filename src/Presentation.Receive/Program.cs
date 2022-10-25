using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Application;
using Application.interfaces;
using Infrastructure;
// 20

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IConfiguration>(_ => configuration)
            .AddMessageReceiver()
            .AddApplication();
    }).Build();

host.Run();