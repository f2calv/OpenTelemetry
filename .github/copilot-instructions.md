# Copilot Instructions

## Shared Instructions

Shared Copilot instructions, skills and prompts are maintained centrally in the
[account-level `.github` repository](https://github.com/f2calv/.github). They are deliberately not
copied here.

To load them, clone that repository and either add it to the VS Code workspace or link its
instruction, skill and prompt folders into `~/.copilot/`. If those files are unavailable, stop
rather than guessing the conventions.

Everything below is specific to this repository.

## Repository Purpose

This repository is a .NET OpenTelemetry playground with console, worker and ASP.NET Core examples.
The local Compose stack supplies Jaeger for observing synthetic development traffic over OTLP.

- Keep instrumentation shared through `SharedLibrary` where the examples use the same behavior.
- Do not add telemetry attributes containing credentials, request bodies or personal data.
- Keep every project on .NET 10 and keep the OpenTelemetry package family aligned when updating the
  instrumentation or exporter set.
