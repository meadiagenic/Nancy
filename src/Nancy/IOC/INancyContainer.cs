namespace Nancy.IOC
{
    using System;

    public interface INancyContainer : IDisposable
    {
        void Register(string name, Type serviceType, object instance);
        void Register(string name, Type serviceType, Func<INancyContainer, object> factory);
        void Register(string name, Type serviceType, Type implementationType);

        void RegisterSingleton(string name, Type serviceType, Func<INancyContainer, object> factory);
        void RegisterSingleton(string name, Type serviceType, Type implementationType);

        object Resolve(string name, Type serviceType);

        bool CanResolve(Type serviceType);
    }
}
