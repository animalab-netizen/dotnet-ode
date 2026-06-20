# Contributing

Thank you for contributing to `dotnet-ode`.

## Local Validation

```bash
dotnet test /Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/dotnet-ode.sln
dotnet pack /Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/src/DotNetODE/DotNetODE.csproj -c Release
```

## Guidelines

- preserve conceptual parity with the ODE family
- keep examples small and explainable
- avoid framework lock-in inside the core runtime
