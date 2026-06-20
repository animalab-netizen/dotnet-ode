using DotNetODE.Business.DTO;

namespace DotNetODE.Business.Interactor;

public static class UseCaseDispatcher
{
    public static async Task<Output<TResult>> DispatchAsync<TParam, TResult>(
        UseCase<TParam, TResult> useCase,
        TParam param,
        Action<Output<TResult>>? publish = null)
    {
        var output = await useCase.ProcessAsync(param).ConfigureAwait(false);
        publish?.Invoke(output);
        return output;
    }
}
