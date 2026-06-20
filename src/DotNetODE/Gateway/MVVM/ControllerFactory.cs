namespace DotNetODE.Gateway.MVVM;

public interface IControllerFactory<out TController>
{
    TController Create();
}

public abstract class ControllerFactory<TController> : IControllerFactory<TController>
{
    public abstract TController Create();
}
