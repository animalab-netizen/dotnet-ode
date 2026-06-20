namespace DotNetODE.Business.DTO;

public abstract record Output<T>;

public sealed record ValueOutput<T>(T Value) : Output<T>;

public sealed record ErrorOutput<T>(System.Exception Error) : Output<T>;

public sealed record EmptyOutput<T>() : Output<T>;
