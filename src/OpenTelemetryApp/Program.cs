using OpenTelemetry;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
namespace CasCap
{
    class Program
    {
        static string sourceName = $"{nameof(CasCap)}.{AppDomain.CurrentDomain.FriendlyName}";
        static readonly ActivitySource MyActivitySource = new ActivitySource(sourceName);

        public static void Main()
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                .AddSource(sourceName)
                .AddConsoleExporter()
                .Build();

            using (var activity = MyActivitySource.StartActivity("SayHello"))
            {
                activity?.SetTag("foo", 1);
                activity?.SetTag("bar", "Hello, World!");
                activity?.SetTag("baz", new int[] { 1, 2, 3 });
            }
        }
    }
}