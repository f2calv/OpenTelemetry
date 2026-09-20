# OpenTelemetry .NET Playground

A collection of .NET 6 examples for experimenting with OpenTelemetry instrumentation and exporters.
It is a learning environment rather than a production observability stack.

## Projects

| Project | Purpose |
| --- | --- |
| `OpenTelemetryApp` | Console application example. |
| `SharedLibrary` | Shared tracing and exporter setup. |
| `testapp` | Additional console-host example. |
| `webapi` | ASP.NET Core API instrumentation example. |
| `workerapp` | Worker service instrumentation example. |

## Prerequisites

- A .NET SDK capable of targeting .NET 6
- Docker with Compose, when running the optional tracing backends

## Run locally

Start the local Jaeger and Zipkin services:

```pwsh
docker compose up -d
```

Jaeger is exposed at `http://localhost:16686` and Zipkin at `http://localhost:9411`.

Restore and build the examples:

```pwsh
dotnet restore .\OpenTelemetry.sln
dotnet build .\OpenTelemetry.sln --no-restore
```

Stop and remove the local containers when finished:

```pwsh
docker compose down
```

Only synthetic development telemetry should be sent to this local stack.

## Resources

- [OpenTelemetry .NET](https://github.com/open-telemetry/opentelemetry-dotnet)
- [Jaeger](https://www.jaegertracing.io/)
- [Zipkin](https://zipkin.io/)
