# Design

## Context

Both projects already target `net10.0` and reference ASP.NET Core / EF Core packages pinned to `10.0.2` (the .NET 10 GA release). The installed SDK is `10.0.401`. `dotnet build` succeeds but reports:
- `NU1903`: `Microsoft.OpenApi 2.0.0` (pulled in transitively by `Microsoft.AspNetCore.OpenApi 10.0.2`) has a high-severity advisory, [GHSA-v5pm-xwqc-g5wc](https://github.com/advisories/GHSA-v5pm-xwqc-g5wc) / CVE-2026-49451 — a crafted OpenAPI document with circular schema references can crash the process (stack overflow) when parsed. Patched at `Microsoft.OpenApi >= 2.7.5` (2.x line).
- `ASPDEPR002`: `WithOpenApi()` is obsolete in .NET 10 (7 call sites in `WiDocApi_test/Endpoints/PersonEndpoints.cs`).

`.gitlab-ci.yml` still includes `dotnet8-pipelines/*` templates from the external `library-systems-and-development/ci-templates` project. See proposal.md for the full motivation.

## Goals / Non-Goals

**Goals:**
- Land on package versions and CI configuration that are consistent with `net10.0` and free of the known `Microsoft.OpenApi` advisory.
- Remove obsolete-API usage introduced by the ASP.NET Core 10 OpenAPI changes.
- Make the SDK version reproducible across dev machines and CI.

**Non-Goals:**
- No change to WiDocApi's public API, Razor components, or runtime behavior.
- No CI template authoring — if the `ci-templates` project has no `.NET 10` template yet, this change only identifies and flags that gap; it does not create the template in the other project.

## Decisions

- **Fix the `Microsoft.OpenApi` advisory by bumping `Microsoft.AspNetCore.OpenApi`, not by adding a direct override.** `Microsoft.AspNetCore.OpenApi 10.0.2` pins `Microsoft.OpenApi` to exactly `2.0.0`. Every referenced package (`Microsoft.AspNetCore.Components.Web`, `Microsoft.AspNetCore.OpenApi`, `Microsoft.AspNetCore.WebUtilities`, `Microsoft.Extensions.Configuration.Abstractions`, `Microsoft.AspNetCore.Components.QuickGrid[.EntityFrameworkAdapter]`, `Microsoft.EntityFrameworkCore.SqlServer`) already has a `10.0.12` patch release available on NuGet, and `Microsoft.AspNetCore.OpenApi 10.0.12` raises its `Microsoft.OpenApi` dependency to `[2.12.0, 3.0.0)`, which is past the `2.7.5` patched floor. Bumping all `10.0.2` → `10.0.12` references together resolves NU1903 without an explicit pin, and keeps the whole ASP.NET Core/EF Core package set on one consistent patch release.
  - *Alternative considered*: add a direct `<PackageReference Include="Microsoft.OpenApi" Version="2.12.0" />` to force the transitive version up while staying on `10.0.2`. Rejected — it leaves the rest of the 10.0.2 packages behind on an older patch release for no reason, and reintroduces the same problem on the next advisory.
- **Replace `WithOpenApi()` with metadata set directly on the route handler** (e.g. `.WithSummary()/.WithDescription()`/`.WithTags()` or `.WithMetadata()` as appropriate per call site), matching the ASPDEPR002 guidance at https://aka.ms/aspnet/deprecate/002, rather than suppressing the warning. `WithOpenApi()` is sample/test code here (`WiDocApi_test`), so there's no external contract to preserve — only equivalent OpenAPI metadata.
- **Pin the SDK with `global.json` using a `rollForward: latestFeature` policy on `10.0.401`** rather than an exact match. Three SDKs are installed on this dev machine (3.1, 8.0, 10.0); an exact pin is the safest default for CI reproducibility, but `latestFeature` avoids breaking local builds on the next SDK patch install. If the team wants strict reproducibility instead, `rollForward: disable` with an exact version is the stricter alternative — flagged as an open question below.

## Risks / Trade-offs

- [Risk] The `ci-templates` project's `dotnet8-pipelines` path may not have a `dotnet10-pipelines` (or equivalent) counterpart yet → Mitigation: this change's tasks include verifying available templates in that project before editing `.gitlab-ci.yml`; if no .NET 10 template exists yet, flag it back to the proposal/task list rather than guessing a path.
- [Risk] Bumping 6 package references at once from `10.0.2` to `10.0.12` (10 patch releases) could theoretically introduce an unrelated regression → Mitigation: `dotnet build` and existing tests (if any) are run after the bump as part of this change's tasks; the jump is within the same major/minor (`10.0.x`), which is the lowest-risk kind of package bump.
- [Risk] `global.json` with `rollForward: latestFeature` still allows a future SDK install to silently change the build's toolchain locally → Mitigation: CI pins its own runner image/SDK independently of this repo's `global.json`; documented as an explicit, accepted trade-off rather than solved here.

## Open Questions

- Should `global.json` use `rollForward: latestFeature` (flexible) or `rollForward: disable` with an exact SDK version (strict, matches CI exactly)? Defaulting to `latestFeature` per the Decisions section; revisit if CI reproducibility issues show up later.
