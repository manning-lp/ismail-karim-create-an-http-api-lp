using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            options.EnableAnnotations();

            // display XML Doc

            Assembly root = Assembly.GetExecutingAssembly();

            // if (root.FullName != null)
            // {
            //    string baseName = root.FullName.Substring(0, root.FullName.IndexOf('.'));
            //    List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //    List<string> xmlCommentFiles =
            //        AppDomain.CurrentDomain
            //        .GetAssemblies()
            //        .Where(
            //            a =>
            //                a.FullName is not null
            //                && a.FullName.StartsWith(baseName, StringComparison.OrdinalIgnoreCase)
            //                && (a.FullName.Contains("Server") || a.FullName.Contains("API")))
            //        .Select(a => { Path.ChangeExtension(assembly.Location, ".xml").ToString(); })
            //        .ToList();

            // foreach (string xmlCommentFile in xmlCommentFiles)
            //    {
            //        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //        options.IncludeXmlComments(xmlPath);
            //    }
            // }

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "RobotsInc Inspections API",
                Description = "This is the RobotsInc Inspections API specification.  This api will be used\r\n "
                + "by both the Inspections web application and by the Android app.",
                Contact = new OpenApiContact
                {
                    Name = "Ismail KARIMALLAH",
                    Url = new Uri("https://github.com/ismail-karim")
                }
            });
            options.IncludeXmlComments(Assembly.GetExecutingAssembly());
        });
        services.AddTransient<IHealthManager, HealthManager>();
        services.AddSingleton<IOfficeHoursManager, OfficeHoursManager>();
        services.AddSingleton<ITimerProvider, TimerProvider>();
    }

    private static void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseEndpoints(endpoint => endpoint.MapControllers());
    }
}
