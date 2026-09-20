using Microsoft.Extensions.Configuration;
using OpenTelemetry;
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

            var jaegerEndpoint = new Uri(configuration["Jaeger:Endpoint"]
                ?? throw new InvalidOperationException("Jaeger:Endpoint is required."));

            var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                // Add more libraries
                .AddSource("Sample")
                // Add more exporters
                .AddConsoleExporter()
                .AddOtlpExporter(options => options.Endpoint = jaegerEndpoint)
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
            using (var activity = s_source.StartActivity("SomeWork"))
            {
                await StepOne();
                await StepTwo();
            }
        }

        static async Task StepOne()
        {
            using (var activity = s_source.StartActivity("StepOne"))
            {
                await Task.Delay(500);
            }
        }

        static async Task StepTwo()
        {
            using (var activity = s_source.StartActivity("StepTwo"))
            {
                await Task.Delay(1000);
            }
        }
    }
}
