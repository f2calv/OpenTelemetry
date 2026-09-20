using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

namespace CasCap
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "webapi", Version = "v1" });
            });

            // Switch exporters by setting UseExporter in appsettings.json.
            var exporter = Configuration.GetValue<string>("UseExporter")?.ToLowerInvariant();
            var openTelemetry = services.AddOpenTelemetry();

            switch (exporter)
            {
                case "jaeger":
                    openTelemetry.ConfigureResource(builder => builder.AddService(
                        Configuration.GetValue<string>("Jaeger:ServiceName") ?? "webapi"));
                    break;
                case "otlp":
                    openTelemetry.ConfigureResource(builder => builder.AddService(
                        Configuration.GetValue<string>("Otlp:ServiceName") ?? "webapi"));
                    break;
            }

            openTelemetry.WithTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                switch (exporter)
                {
                    case "jaeger":
                        builder.AddOtlpExporter(options => options.Endpoint = new Uri(
                            Configuration.GetValue<string>("Jaeger:Endpoint")
                                ?? throw new InvalidOperationException("Jaeger:Endpoint is required.")));
                        break;
                    case "otlp":
                        builder.AddOtlpExporter(options => options.Endpoint = new Uri(
                            Configuration.GetValue<string>("Otlp:Endpoint")
                                ?? throw new InvalidOperationException("Otlp:Endpoint is required.")));
                        break;
                    default:
                        builder.AddConsoleExporter();
                        break;
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "webapi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
