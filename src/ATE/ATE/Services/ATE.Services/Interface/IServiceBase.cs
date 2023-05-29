namespace ATE.Service.Interface
{
    public enum ServiceState
    {
        Uninit,
        Running,
        Paused, 
        Stop,
        Error
    }

    public interface IServiceBase
    {
        string Name { get; }
        string Description { get; }
        ServiceState ServiceState { get; }
    }
}
