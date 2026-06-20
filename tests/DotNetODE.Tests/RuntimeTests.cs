using DotNetODE.Business.DTO;
using DotNetODE.Business.Exception;
using DotNetODE.Business.Interactor;

namespace DotNetODE.Tests;

public sealed class RuntimeTests
{
    [Fact]
    public async Task DirectUseCaseNormalizesRawValues()
    {
        var output = await new EchoUseCase().ProcessAsync("pikachu");

        var value = Assert.IsType<ValueOutput<string>>(output);
        Assert.Equal("echo:pikachu", value.Value);
    }

    [Fact]
    public async Task GuardedUseCaseBlocksInvalidRequests()
    {
        var output = await new GuardedUseCase().ProcessAsync(string.Empty);

        var error = Assert.IsType<ErrorOutput<string>>(output);
        Assert.IsType<GuardRejectedError>(error.Error);
    }

    [Fact]
    public async Task ChainUseCaseMapsFirstResultIntoSecondStory()
    {
        var chained = new ChainUseCase<string, string, string>(
            new EchoUseCase(),
            (value, _) => Task.FromResult<ExecutionResult<string>>(Outputs.Value($"{value}:story")));

        var output = await chained.ProcessAsync("bulbasaur");

        var value = Assert.IsType<ValueOutput<string>>(output);
        Assert.Equal("echo:bulbasaur:story", value.Value);
    }

    [Fact]
    public async Task SequenceUseCasePreservesOrderedEntries()
    {
        var sequence = new SequenceUseCase<string, string>((value) => Task.FromResult(value.ToUpperInvariant()));

        var output = await sequence.ProcessAsync(new[] { "bulbasaur", "charmander", "squirtle" });

        var value = Assert.IsType<ValueOutput<IReadOnlyList<string>>>(output);
        Assert.Equal(new[] { "BULBASAUR", "CHARMANDER", "SQUIRTLE" }, value.Value);
    }

    private sealed class EchoUseCase : UseCase<string, string>
    {
        protected override Task<ExecutionResult<string>> ExecuteAsync(string param) =>
            Task.FromResult<ExecutionResult<string>>($"echo:{param}");
    }

    private sealed class GuardedUseCase : UseCase<string, string>
    {
        protected override Task<GuardResult> GuardAsync(string param) =>
            Task.FromResult(string.IsNullOrWhiteSpace(param)
                ? GuardResult.Deny(new GuardRejectedError("missing param"))
                : GuardResult.Allow());

        protected override Task<ExecutionResult<string>> ExecuteAsync(string param) =>
            Task.FromResult<ExecutionResult<string>>(param.ToUpperInvariant());
    }
}
