namespace ManagerActorFramework
{
    public delegate void Subscription<TManager>(object[] arguments) where TManager : Manager<TManager>;
}