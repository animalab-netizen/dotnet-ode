# Publication Guide

## Current State

`dotnet-ode` is structured as a standalone NuGet package.

Current coordinates:

- package: `dotnet-ode`
- version: `0.1.1`
- repository: [github.com/animalab-netizen/dotnet-ode](https://github.com/animalab-netizen/dotnet-ode)

## Trusted Publishing

`dotnet-ode` is now configured for NuGet Trusted Publishing from GitHub Actions.

Repository-side workflow:

- workflow file: `.github/workflows/release.yml`
- provider model: GitHub Actions OIDC
- publish trigger: push tag `v*`
- required workflow permission: `id-token: write`
- package push command: `dotnet nuget push` to `https://api.nuget.org/v3/index.json`

NuGet.org still needs the matching Trusted Publisher entry created in the package owner account. In practice, that means linking the `animalab-netizen/dotnet-ode` repository to the release workflow above in the NuGet.org Trusted Publishing UI. No NuGet API key should be stored in GitHub Secrets for this flow.

## Source Repository

- repository: [github.com/animalab-netizen/dotnet-ode](https://github.com/animalab-netizen/dotnet-ode)
- git url: `https://github.com/animalab-netizen/dotnet-ode.git`
- package id: `dotnet-ode`

## Distribution Model

The package is intended for:

- direct NuGet distribution as the public .NET ODE runtime
- consumption by community showcases such as `dotnet-ode-consumer`
- installation without any private company-specific domain once public publication is enabled

## Installation

```xml
<PackageReference Include="dotnet-ode" Version="0.1.1" />
```

## Release Checklist

### GitHub Release Gate

1. Run `dotnet test /Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/dotnet-ode.sln`
2. Run `dotnet pack /Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/src/DotNetODE/DotNetODE.csproj -c Release`
3. Confirm CI is green in `.github/workflows/ci.yml`
4. Update `CHANGELOG.md`
5. Confirm version in `DotNetODE.csproj`
6. Commit release metadata
7. Create and push tag `v0.1.1`

### Public Package Gate

1. Confirm the package name remains `dotnet-ode`
2. In NuGet.org, create or verify the Trusted Publisher entry for `animalab-netizen/dotnet-ode`
3. Confirm the GitHub workflow file is `.github/workflows/release.yml`
4. Confirm the workflow job has `id-token: write`
5. Push tag `v0.1.1`
6. Verify the package page on NuGet
7. Validate installation from a clean consumer with `<PackageReference Include="dotnet-ode" Version="0.1.1" />`
8. Publish release notes with install and usage examples

## Packaging Notes

- the package ships from `src/DotNetODE`
- XML documentation and symbols are generated for package consumers
- the runtime has no third-party production dependencies
- package metadata is embedded in `DotNetODE.csproj`
- no NuGet API key is required once NuGet Trusted Publishing is correctly linked to the repository workflow
