using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Microservices.Demo.Core.Elasticsearch
{
    public static class Extensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            ElasticsearchOptions elasticsearchOptions = new ElasticsearchOptions();
            IConfigurationSection configurationSection = configuration.GetSection("elasticsearch");
            configurationSection.Bind(elasticsearchOptions);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", elasticsearchOptions.ApplicationName)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(
                        new Uri(elasticsearchOptions.Url))
                    {
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                        AutoRegisterTemplate = true,
                        TemplateName = "serilog-events-template",
                        IndexFormat = elasticsearchOptions.IndexFormat
                    })
                .MinimumLevel.Verbose()
                .CreateLogger();
        }
    }
}
