namespace Nancy.IOC
{
    using System;

    public static class INancyContainerExtensions
    {
        public static bool Contains<T>(this INancyContainer container)
        {
            return container.CanResolve(typeof(T));
        }

        public static object Resolve(this INancyContainer container, Type serviceType) {
            return container.Resolve(string.Empty, serviceType);
        }

        public static T Resolve<T>(this INancyContainer container)
        {
            return (T)container.Resolve(string.Empty, typeof(T));
        }

        public static void Register(this INancyContainer container, Type serviceType, object instance)
        {
            container.Register(string.Empty, serviceType, instance);
        }

        public static void Register<TService>(this INancyContainer container, TService instance)
        {
            container.Register(typeof(TService), instance);
        }

        public static void Register<TService>(this INancyContainer container, Type implementationType)
        {
            container.Register(typeof(TService), implementationType);
        }

        public static void Register(this INancyContainer container, Type serviceType, Func<INancyContainer, object> factory)
        {
            container.Register(string.Empty, serviceType, factory);
        }

        public static void Register(this INancyContainer container, Type serviceType, Type implementationType)
        {
            container.Register(string.Empty, serviceType, implementationType);
        }

        public static void Register<TService, TImplementation>(this INancyContainer container) where TImplementation : TService
        {
            container.Register<TService, TImplementation>(string.Empty);
        }

        public static void Register<TService, TImplementation>(this INancyContainer container, string name) where TImplementation : TService
        {
            container.Register(name, typeof(TService), typeof(TImplementation));
        }

        public static void RegisterIfNone<TService, TImplementation>(this INancyContainer container) where TImplementation : TService
        {
            if (!container.Contains<TService>())
            {
                container.Register<TService, TImplementation>();
            }
        }


        public static void RegisterIfNone<TService>(this INancyContainer container, TService instance)
        {
            if (!container.Contains<TService>())
            {
                container.Register<TService>(instance);
            }
        }

        public static void RegisterSingleton<TService, TImplementation>(this INancyContainer container) where TImplementation : TService
        {
            container.RegisterSingleton<TService, TImplementation>(string.Empty);
        }

        public static void RegisterSingleton<TService, TImplemenation>(this INancyContainer container, string name) where TImplemenation : TService
        {
            container.RegisterSingleton(name, typeof(TService), typeof(TImplemenation));
        }

        public static void RegisterSingletonIfNone<TService, TImplementation>(this INancyContainer container) where TImplementation : TService
        {
            if (!container.Contains<TService>())
            {
                container.RegisterSingleton<TService, TImplementation>();
            }
        }

    }
}
