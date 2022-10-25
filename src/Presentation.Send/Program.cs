using Application.interfaces;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// 13 Loads settings from appsettings.
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add configuration to ServiceCollection so that RabbitMQSender can inject it.
// "AddMessenger" is an extension method on ServiceCollection.
// "BuildServiceProvider" initiates the serviceProvider
var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(_ => configuration)
    .AddMessenger()
    .BuildServiceProvider();

// We are geting whatever implementation of the contract.
var messageSender = serviceProvider
    .GetService<IMessageSender>()!;

string name = " ";

while (!string.IsNullOrEmpty(name))
{
    Console.WriteLine("What is your name? (new line to exit)");
    name = Console.ReadLine()!;

    messageSender.Send($"Hello my name is, {name}");
}