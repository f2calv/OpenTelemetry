# OpenTelemetry .NET Playground

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=f2calv_OpenTelemetry&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=f2calv_OpenTelemetry)

A collection of .NET 10 examples for experimenting with OpenTelemetry instrumentation and exporters.
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

- A .NET 10 SDK
- Docker with Compose, when running the optional tracing backend

## Run locally

Start the local Jaeger service:

```pwsh
docker compose up -d
```

Jaeger is exposed at `http://localhost:16686` and accepts OTLP on ports `4317` and `4318`.

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
