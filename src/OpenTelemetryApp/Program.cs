using Examples.Console;
using OpenTelemetry;
using OpenTelemetry.Resources;
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
            //using var openTelemetry = Sdk.CreateTracerProviderBuilder()
            //    .SetSampler(new AlwaysOnSampler())
            //    .AddSource(sourceName)
            //    .AddConsoleExporter()
            //    .Build();

            //using (var activity = MyActivitySource.StartActivity("SayHello"))
            //{
            //    activity?.SetTag("foo", 1);
            //    activity?.SetTag("bar", "Hello, World!");
            //    activity?.SetTag("baz", new int[] { 1, 2, 3 });
            //}

            using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("jaeger-test"))
                    .AddSource("Samples.SampleClient", "Samples.SampleServer")
                    .AddJaegerExporter(o =>
                    {
                        o.AgentHost = "localhost";
                        o.AgentPort = 16686;
                    })
                    .Build();

            // The above lines are required only in Applications which decide to use OpenTelemetry.

            using (var sample = new InstrumentationWithActivitySource())
            {
                sample.Start();

                Console.WriteLine("Traces are being created and exported" +
                    "to Jaeger in the background. Use Jaeger to view them." +
                    "Press ENTER to stop.");
                Console.ReadLine();
            }
        }
    }
}