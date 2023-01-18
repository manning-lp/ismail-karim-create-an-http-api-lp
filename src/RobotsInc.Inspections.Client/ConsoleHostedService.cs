using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RobotsInc.Inspections.Client;

/// <summary>
///     Single <see cref="IHostedService" /> for a console application.
/// </summary>
public class ConsoleHostedService : BackgroundService
{
    private readonly InspectionsApiOptions _inspectionsApiOptions;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ConsoleHostedService> _logger;

    public ConsoleHostedService(
        IOptions<InspectionsApiOptions> inspectionsApiOptions,
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IHttpClientFactory httpClientFactory)
    {
        _inspectionsApiOptions = inspectionsApiOptions.Value;
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string HealthPath = "/api/v1/health";

        // don't block during start of IHostedService
        await Task.Yield();

        // execute
        try
        {
            _logger.LogInformation($"{nameof(ConsoleHostedService)} started execution.");

            using HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_inspectionsApiOptions.BaseAddress);
            httpClient.Timeout = _inspectionsApiOptions.Timeout;

            using HttpRequestMessage request =
                new()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(HealthPath, UriKind.Relative),
                };

            using HttpResponseMessage response =
                await httpClient.SendAsync(request, stoppingToken);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync(stoppingToken);
                Console.WriteLine($"Call succeeded with status code: {response.StatusCode.ToString()}");
                Console.WriteLine($"Response body: <{json}>");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Call failed with status code: {response.StatusCode.ToString()}");
                Console.WriteLine($"The endpoint was not found on the path: {HealthPath}");
            }
            else if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                string json = await response.Content.ReadAsStringAsync(stoppingToken);
                Console.WriteLine($"Service is not available, answer with status code: {response.StatusCode.ToString()}");
                Console.WriteLine($"Response body: <{json}>");
            }
            else
            {
                Console.WriteLine($"Unexpected status code: {response.StatusCode.ToString()}");
            }

            _logger.LogInformation($"{nameof(ConsoleHostedService)} stopped execution.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Aborted because of error.");
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }
}
