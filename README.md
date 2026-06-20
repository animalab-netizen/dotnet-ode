# dotnet-ode

`dotnet-ode` is the .NET member of the ODE architecture family.

The package provides a compact runtime for:

- use case execution
- guard-first dispatch flow
- explicit output publication
- chained and sequenced orchestration
- lightweight MVVM-style channels for viewmodel driven UI code

## Repository

- source: [github.com/animalab-netizen/dotnet-ode](https://github.com/animalab-netizen/dotnet-ode)

## Status

`dotnet-ode` is prepared as a standalone publishable NuGet package and is intended to be consumed by showcase applications such as `dotnet-ode-consumer`.

The package is maintained by ÂnimaLab and is being positioned as the Microsoft and .NET expression of the same ODE vocabulary already available in Kotlin, Swift and TypeScript.

## Coordinates

Current coordinates:

- package: `dotnet-ode`
- version: `0.1.2`

Installation:

```xml
<PackageReference Include="dotnet-ode" Version="0.1.2" />
```

## Public API

The intended public surface of `dotnet-ode` is centered on these concepts:

- `UseCase<TParam, TResult>`
- `UseCaseDispatcher`
- `Output<T>`, `ValueOutput<T>`, `ErrorOutput<T>`, `EmptyOutput<T>`, `Outputs`
- `BaseViewModel`
- `Channel<T>`
- `Controller`
- `ControllerFactory<TController>`
- `SequenceUseCase<TParam, TResult>`
- `ChainUseCase<TParam, TIntermediate, TResult>`
- `HttpError`, `ConnectionError`, `GuardRejectedError`, `UnexpectedResponseError`

Internal helpers should not be treated as product contract and may change without notice.

## Core Concepts

### 1. UseCase

`UseCase<TParam, TResult>` is the main business execution abstraction.

It provides a standard lifecycle for:

- input validation via `GuardAsync`
- execution via `ExecuteAsync`
- result normalization via `OnResult`
- failure handling via `OnError`

### 2. Outputs

`dotnet-ode` uses an explicit output hierarchy:

- `ValueOutput<T>`
- `ErrorOutput<T>`
- `EmptyOutput<T>`

### 3. ChainUseCase

`ChainUseCase` is intended for a two-step flow where the first successful result provides the context for the second step.

### 4. SequenceUseCase

`SequenceUseCase` is intended for ordered execution across three or more entries.

### 5. BaseViewModel

`BaseViewModel` provides:

- typed channel creation
- observation registration
- output publication through named channels
- direct use case dispatch into a chosen channel

## Basic Examples

### Direct UseCase

```csharp
using DotNetODE.Business.Interactor;

public sealed class LoadPokemonUseCase : UseCase<string, string>
{
    protected override Task<ExecutionResult<string>> ExecuteAsync(string param) =>
        Task.FromResult<ExecutionResult<string>>($"spotlight:{param}");
}
```

### Guarded UseCase

```csharp
using DotNetODE.Business.Exception;
using DotNetODE.Business.Interactor;

public sealed class ComparePokemonUseCase : UseCase<(string Left, string Right), string>
{
    protected override Task<GuardResult> GuardAsync((string Left, string Right) param)
    {
        if (string.IsNullOrWhiteSpace(param.Left) || string.IsNullOrWhiteSpace(param.Right) || param.Left == param.Right)
        {
            return Task.FromResult(GuardResult.Deny(new GuardRejectedError("Comparison requires two distinct pokemon.")));
        }

        return Task.FromResult(GuardResult.Allow());
    }

    protected override Task<ExecutionResult<string>> ExecuteAsync((string Left, string Right) param) =>
        Task.FromResult<ExecutionResult<string>>($"{param.Left} vs {param.Right}");
}
```

### ViewModel Example

```csharp
using DotNetODE.Business.DTO;
using DotNetODE.Business.Interactor;
using DotNetODE.Gateway.MVVM;

public sealed class DemoViewModel : BaseViewModel
{
    public Channel<Output<string>> NameChannel { get; } = new("name");

    public Task<Output<string>> LoadAsync() =>
        DispatchUseCaseAsync(Unit.Value, new LoadNameUseCase(), NameChannel);

    private sealed class LoadNameUseCase : UseCase<Unit, string>
    {
        protected override Task<ExecutionResult<string>> ExecuteAsync(Unit param) =>
            Task.FromResult<ExecutionResult<string>>("pikachu");
    }
}
```

## Publishing

Local validation:

```bash
dotnet test /Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/dotnet-ode.sln
dotnet pack /Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/src/DotNetODE/DotNetODE.csproj -c Release
```

See [PUBLICATION.md](/Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/PUBLICATION.md) for the release checklist and packaging notes.

## Contributing

See [CONTRIBUTING.md](/Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/CONTRIBUTING.md).

## Changelog

See [CHANGELOG.md](/Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/CHANGELOG.md).

## Maintainer

- name: `ÂnimaLab`
- email: `animalab.desenvolvimento@gmail.com`

## License

This project is licensed under Apache-2.0. See [LICENSE](/Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/LICENSE).

## UseCase Guide

See [USECASE_GUIDE.md](/Users/caiosanchezchristino/Desktop/ode-projects/dotnet-ode/USECASE_GUIDE.md) for combinations, adoption guidance and common implementation doubts.
