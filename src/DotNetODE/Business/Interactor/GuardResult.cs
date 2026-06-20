namespace DotNetODE.Business.Interactor;

public readonly record struct GuardResult(bool IsAllowed, System.Exception? Error)
{
    public static GuardResult Allow() => new(true, null);

    public static GuardResult Deny(System.Exception error) => new(false, error);
}
