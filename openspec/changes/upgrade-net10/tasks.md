# Tasks

## 1. Package version bump

- [x] 1.1 Bump every `10.0.2` `PackageReference` in `WiDocApi_Blazor/WiDocApi_Blazor.csproj` (`Microsoft.AspNetCore.Components.Web`, `Microsoft.AspNetCore.OpenApi`, `Microsoft.AspNetCore.WebUtilities`, `Microsoft.Extensions.Configuration.Abstractions`) to `10.0.12` and verify `dotnet restore` succeeds.
- [x] 1.2 Bump every `10.0.2` `PackageReference` in `WiDocApi_test/WiDocApi_test.csproj` (`Microsoft.AspNetCore.Components.QuickGrid`, `Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter`, `Microsoft.AspNetCore.OpenApi`, `Microsoft.EntityFrameworkCore.SqlServer`) to `10.0.12` and verify `dotnet restore` succeeds.
- [x] 1.3 Run `dotnet build WiDocApi.sln` and confirm the `NU1903` (`Microsoft.OpenApi` advisory) warning is gone.

## 2. Obsolete API cleanup

- [x] 2.1 Replace the 7 `WithOpenApi()` calls in `WiDocApi_test/Endpoints/PersonEndpoints.cs` (lines ~42, 61, 90, 114, 132, 146, 163) with the non-obsolete .NET 10 equivalents per https://aka.ms/aspnet/deprecate/002, preserving the same OpenAPI metadata (summary/description/tags) each call currently sets.
- [x] 2.2 Run `dotnet build WiDocApi.sln` and confirm no `ASPDEPR002` warnings remain.

## 3. Metadata cleanup

- [x] 3.1 Update `<Title>WiDocApi_blazor Net 8.0</Title>` in `WiDocApi_Blazor/WiDocApi_Blazor.csproj` to reference .NET 10, and verify by re-reading the file.

## 4. SDK pinning

- [x] 4.1 Add a `global.json` at the repo root pinning `"version": "10.0.401"` with `"rollForward": "latestFeature"`, and verify `dotnet --version` inside the repo root resolves to that SDK.

## 5. CI pipeline

- [x] 5.1 Check the `library-systems-and-development/ci-templates` GitLab project for a `.NET 10` equivalent of `dotnet8-pipelines/nuget-tag-pipeline.yml` and `dotnet8-pipelines/merge-request-pipeline.yml`.
- [x] 5.2 If a .NET 10 template exists, update `.gitlab-ci.yml`'s two `include` paths to it and verify the pipeline runs green on this branch (or the next pushed commit). If no such template exists yet, leave `.gitlab-ci.yml` on `dotnet8-pipelines` and record that gap for the CI templates project instead of guessing a path.

## 6. Final verification

- [x] 6.1 Run `dotnet build WiDocApi.sln` and confirm 0 errors, and that the `NU1903` and `ASPDEPR002` warnings from the pre-upgrade baseline are gone.
- [x] 6.2 Run any existing test suite (`dotnet test`, if test projects exist) and confirm it passes.
