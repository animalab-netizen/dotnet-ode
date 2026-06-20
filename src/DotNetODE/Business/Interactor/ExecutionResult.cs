using DotNetODE.Business.DTO;

namespace DotNetODE.Business.Interactor;

public readonly struct ExecutionResult<TResult>
{
    private ExecutionResult(TResult value)
    {
        Value = value;
        Output = null;
        IsOutput = false;
    }

    private ExecutionResult(Output<TResult> output)
    {
        Value = default;
        Output = output;
        IsOutput = true;
    }

    public TResult? Value { get; }

    public Output<TResult>? Output { get; }

    public bool IsOutput { get; }

    public static implicit operator ExecutionResult<TResult>(TResult value) => new(value);

    public static implicit operator ExecutionResult<TResult>(Output<TResult> output) => new(output);
}
