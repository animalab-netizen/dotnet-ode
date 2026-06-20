using DotNetODE.Business.DTO;

namespace DotNetODE.Business.Interactor;

public class ChainUseCase<TParam, TIntermediate, TResult> : UseCase<TParam, TResult>
{
    private readonly UseCase<TParam, TIntermediate> _first;
    private readonly Func<TIntermediate, TParam, Task<ExecutionResult<TResult>>> _second;

    public ChainUseCase(
        UseCase<TParam, TIntermediate> first,
        Func<TIntermediate, TParam, Task<ExecutionResult<TResult>>> second)
    {
        _first = first;
        _second = second;
    }

    protected override async Task<ExecutionResult<TResult>> ExecuteAsync(TParam param)
    {
        var firstOutput = await _first.ProcessAsync(param).ConfigureAwait(false);
        return firstOutput switch
        {
            ValueOutput<TIntermediate> value => await _second(value.Value, param).ConfigureAwait(false),
            ErrorOutput<TIntermediate> error => Outputs.Error<TResult>(error.Error),
            EmptyOutput<TIntermediate> => Outputs.Empty<TResult>(),
            _ => Outputs.Empty<TResult>()
        };
    }
}
