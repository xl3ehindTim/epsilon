using System.Net.Http.Headers;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Converter;
using Epsilon.Canvas.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Epsilon.Canvas;

public static class CanvasServiceCollectionExtensions
{
    private const string CanvasHttpClient = "CanvasHttpClient";

    public static IServiceCollection AddCanvas(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CanvasSettings>(config);
        services.AddHttpClient(
            CanvasHttpClient, static (provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptions<CanvasSettings>>().Value;

                client.BaseAddress = settings.ApiUrl;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.AccessToken);
            });

        services.AddHttpClient<IPaginatorService, PaginatorService>(CanvasHttpClient);
        services.AddHttpClient<IModuleService, ModuleService>(CanvasHttpClient);
        services.AddHttpClient<IAssignmentService, AssignmentService>(CanvasHttpClient);
        services.AddHttpClient<IOutcomeService, OutcomeService>(CanvasHttpClient);
        services.AddHttpClient<ISubmissionService, SubmissionService>(CanvasHttpClient);

        services.AddScoped<ILinkHeaderConverter, LinkHeaderConverter>();
        
        services.AddScoped<ICanvasModuleCollectionFetcher, CanvasModuleCollectionFetcher>();

        return services;
    }
}