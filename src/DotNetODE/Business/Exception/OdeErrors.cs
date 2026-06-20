namespace DotNetODE.Business.Exception;

public sealed class GuardRejectedError : System.Exception
{
    public GuardRejectedError(string message) : base(message)
    {
    }
}

public sealed class HttpError : System.Exception
{
    public HttpError(int status, string url) : base($"The remote endpoint {url} responded with status {status}.")
    {
        Status = status;
        Url = url;
    }

    public int Status { get; }

    public string Url { get; }
}

public sealed class ConnectionError : System.Exception
{
    public ConnectionError(string url, string? detail = null)
        : base(detail is null ? $"Unable to reach {url}." : $"Unable to reach {url}: {detail}")
    {
        Url = url;
    }

    public string Url { get; }
}

public sealed class UnexpectedResponseError : System.Exception
{
    public UnexpectedResponseError(string message) : base(message)
    {
    }
}
