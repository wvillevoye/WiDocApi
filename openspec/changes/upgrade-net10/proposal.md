# Proposal

## Why

The `UPGRADE-NET10` branch has already retargeted both projects to `net10.0` and bumped the ASP.NET Core package references to `10.0.2`, and the solution builds successfully against the installed .NET 10.0.401 SDK. However, the upgrade is not yet complete: leftover metadata still advertises .NET 8, a package dependency carries a known high-severity vulnerability, sample code uses an API marked obsolete under .NET 10, and the GitLab CI pipeline is still wired to `.NET 8` pipeline templates. Finishing these items is what turns "compiles on net10.0" into a properly verified, shippable upgrade.

## What Changes

- Update leftover .NET 8 references in project metadata (`WiDocApi_Blazor.csproj` `<Title>` still reads "WiDocApi_blazor Net 8.0") to reflect .NET 10.
- Bump `Microsoft.OpenApi` past `2.0.0` to resolve the NU1903 high-severity advisory (GHSA-v5pm-xwqc-g5wc) surfaced during `dotnet build`.
- Replace the obsolete `WithOpenApi()` calls (ASPDEPR002, 7 occurrences in `WiDocApi_test/Endpoints/PersonEndpoints.cs`) with the supported .NET 10 OpenAPI metadata approach.
- Update `.gitlab-ci.yml`, which currently includes `dotnet8-pipelines/nuget-tag-pipeline.yml` and `dotnet8-pipelines/merge-request-pipeline.yml` from the `ci-templates` project, to the equivalent .NET 10 pipeline templates (or confirm with the CI templates project whether the dotnet8 templates are still SDK-appropriate).
- Add a `global.json` pinning the .NET 10 SDK version (`10.0.401` or a rolling-forward equivalent), so CI and local builds resolve the same SDK now that multiple SDKs (3.1, 8.0, 10.0) are installed on the machine. **BREAKING** for anyone building this repo without the .NET 10 SDK installed.
- Verify and, where needed, fix any remaining nullable-reference warnings (`CS8604`) touched while working through the above so the build stays warning-clean for the parts we touch.

## Capabilities

This is a pure dependency/tooling/CI upgrade: it does not change WiDocApi's public behavior, API surface, or contracts for consumers of the `WiDocApi_blazor` package. No new or modified capability specs are introduced (`skip_specs: true` set in `.openspec.yaml`).

### New Capabilities
(none)

### Modified Capabilities
(none)

## Impact

- **Projects**: `WiDocApi_Blazor/WiDocApi_Blazor.csproj`, `WiDocApi_test/WiDocApi_test.csproj`.
- **Dependencies**: `Microsoft.OpenApi` (transitive via `Microsoft.AspNetCore.OpenApi`/EF Core OpenAPI tooling) needs a version bump.
- **Sample code**: `WiDocApi_test/Endpoints/PersonEndpoints.cs` (obsolete API usage).
- **CI/CD**: `.gitlab-ci.yml` and the external `library-systems-and-development/ci-templates` project's pipeline selection.
- **Repo root**: new `global.json` to pin the SDK.
- **Consumers**: none expected to break; the published `WiDocApi_blazor` NuGet package's public API is unchanged. Consumers still on .NET 8 will need to upgrade to consume future package versions built against `net10.0`.
