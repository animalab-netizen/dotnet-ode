using DotNetODE.Business.DTO;

namespace DotNetODE.Business.Interactor;

public sealed class SequenceUseCase<TParam, TResult>
{
    private readonly Func<TParam, Task<TResult>> _step;

    public SequenceUseCase(Func<TParam, Task<TResult>> step)
    {
        _step = step;
    }

    public async Task<Output<IReadOnlyList<TResult>>> ProcessAsync(IEnumerable<TParam> values)
    {
        var orderedValues = values.ToList();
        if (orderedValues.Count == 0)
        {
            return Outputs.Empty<IReadOnlyList<TResult>>();
        }

        try
        {
            var results = new List<TResult>(orderedValues.Count);
            foreach (var value in orderedValues)
            {
                results.Add(await _step(value).ConfigureAwait(false));
            }

            return Outputs.Value<IReadOnlyList<TResult>>(results);
        }
        catch (System.Exception error)
        {
            return Outputs.Error<IReadOnlyList<TResult>>(error);
        }
    }
}
