namespace Nancy.IOC
{
    using System;

    public static class INancyContainerExtensions
    {
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

        public static void Register(this INancyContainer container, Type serviceType, Func<INancyContainer, object> factory)
        {
            container.Register(string.Empty, serviceType, factory);
        }

        public static void Register(this INancyContainer container, Type serviceType, Type implementationType)
        {
            container.Register(string.Empty, serviceType, implementationType);
        }
    }
}
