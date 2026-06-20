namespace DotNetODE.Business.DTO;

public static class Outputs
{
    public static ValueOutput<T> Value<T>(T value) => new(value);

    public static ErrorOutput<T> Error<T>(System.Exception error) => new(error);

    public static EmptyOutput<T> Empty<T>() => new();
}
