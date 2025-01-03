using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RobotsInc.Inspections.BusinessLogic.Health;
using RobotsInc.Inspections.BusinessLogic.Utils;
using RobotsInc.Inspections.Server;
namespace RobotsInc.Inspections.Host;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"Running {typeof(Program).Namespace}");
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        Configure(builder.Services);
        builder.Logging.AddConsole();
        WebApplication app = builder.Build();
        Configure(app);
        app.Run();
    }

    private static void Configure(IServiceCollection services)
    {
        var assembly = typeof(HealthController).Assembly;
        services
            .AddControllers()
            .AddApplicationPart(assembly)
            .AddJsonOptions(
                options =>
                {
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.Converters
                    .Add(new JsonStringEnumConverter(null, false));
                });
        services.AddTransient<IHealthManager, HealthManager>();
        services.AddSingleton<IOfficeHoursManager, OfficeHoursManager>();
        services.AddSingleton<ITimerProvider, TimerProvider>();
    }

    private static void Configure(WebApplication app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoint => endpoint.MapControllers());
    }
}
