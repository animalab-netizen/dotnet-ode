using DotNetODE.Business.DTO;

namespace DotNetODE.View.UI;

public static class OutputHandlingStrategy
{
    public static void Handle<T>(
        Output<T> output,
        Action<T> onValue,
        Action<System.Exception> onError,
        Action onEmpty)
    {
        switch (output)
        {
            case ValueOutput<T> value:
                onValue(value.Value);
                break;
            case ErrorOutput<T> error:
                onError(error.Error);
                break;
            default:
                onEmpty();
                break;
        }
    }
}
