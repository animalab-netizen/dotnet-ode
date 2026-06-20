using DotNetODE.Business.DTO;
using DotNetODE.Business.Exception;

namespace DotNetODE.Business.Interactor;

public abstract class UseCase<TParam, TResult>
{
    public async Task<Output<TResult>> ProcessAsync(TParam param)
    {
        var guard = await GuardAsync(param).ConfigureAwait(false);
        if (!guard.IsAllowed)
        {
            return OnError(guard.Error ?? new GuardRejectedError("Guard rejected the dispatch."));
        }

        try
        {
            var execution = await ExecuteAsync(param).ConfigureAwait(false);
            if (execution.IsOutput)
            {
                return execution.Output!;
            }

            return OnResult(execution.Value!);
        }
        catch (System.Exception error)
        {
            return OnError(error);
        }
    }

    protected virtual Task<GuardResult> GuardAsync(TParam param) => Task.FromResult(GuardResult.Allow());

    protected abstract Task<ExecutionResult<TResult>> ExecuteAsync(TParam param);

    protected virtual Output<TResult> OnResult(TResult result) => Outputs.Value(result);

    protected virtual Output<TResult> OnError(System.Exception error) => Outputs.Error<TResult>(error);
}
