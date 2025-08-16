// See https://aka.ms/new-console-template for more information
using Greeter;
using Greeter.Services;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

ILog log = LogManager.GetLogger(typeof(Program));
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly()!);
log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options => 
{
    options.ServiceName = "Greeter";
});

//LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IGreetingService, GreetingService>();

log.Info("Greeter application started.");
IHost host = builder.Build();
var config = host.Services.GetRequiredService<IConfiguration>();

await host.RunAsync();