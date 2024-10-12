using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace RobotsInc.Inspections.Host;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"Running {typeof(Program).Namespace}");
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        Configure(builder.Services);
        WebApplication app = builder.Build();
        Configure(app);
    }

    private static void Configure(IServiceCollection services)
    {
        // methods
    }

    private static void Configure(WebApplication app)
    {
        app.UseRouting();
        app.UseEndpoints(e => { });
        app.Run();
    }
}
