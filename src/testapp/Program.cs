using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
namespace testapp
{
    class Program
    {
        static ActivitySource s_source = new ActivitySource("Sample");

        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var zipkinOptions = configuration.GetSection("zipkin").Get<ZipkinExporterOptions>();
            var jaegerOptions = configuration.GetSection("jaeger").Get<JaegerExporterOptions>();

            var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                // Add more libraries
                .AddSource("Sample")
                // Add more exporters
                .AddConsoleExporter()
                .AddZipkinExporter(zipkinOptions =>
                {
                    zipkinOptions.Endpoint = zipkinOptions.Endpoint;
                })
                .AddJaegerExporter(o =>
                {
                    o.AgentHost = jaegerOptions.AgentHost;
                    o.AgentPort = jaegerOptions.AgentPort;
                })
                .Build();

            while (true)
            {
                await DoSomeWork();
                Console.WriteLine("Example work done");
                await Task.Delay(60_000);
            }
        }

        static async Task DoSomeWork()
        {
            using (Activity? activity = s_source.StartActivity("SomeWork"))
            {
                await StepOne();
                await StepTwo();
            }
        }

        static async Task StepOne()
        {
            using (Activity? activity = s_source.StartActivity("StepOne"))
            {
                await Task.Delay(500);
            }
        }

        static async Task StepTwo()
        {
            using (Activity? activity = s_source.StartActivity("StepTwo"))
            {
                await Task.Delay(1000);
            }
        }
    }
}