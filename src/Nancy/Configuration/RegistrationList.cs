using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC;

namespace Nancy.Configuration
{
    public class RegistrationList : List<RegistrationData>
    {
        public static Func<Type, Type, bool> DefaultFilter = (serviceType, registrationType) =>
        {
            return registrationType.IsAssignableFrom(serviceType) &&
                serviceType != registrationType &&
                !serviceType.IsAbstract &&
                !serviceType.IsGenericTypeDefinition &&
                !serviceType.ContainsGenericParameters;
        };

        public static Action<INancyContainer, Type, Type> RegisterHandler = (container, serviceType, implementationType) => container.Register(implementationType.FullName, serviceType, implementationType);
        public static Action<INancyContainer, Type, Type> RegisterSingletonHandler = (container, serviceType, implementationType) => container.RegisterSingleton(implementationType.FullName, serviceType, implementationType);
        public static Action<INancyContainer, Type, Type> RegisterIfNoneHandler = (container, serviceType, implementationType) =>
        {
            if (!container.CanResolve(serviceType))
            {
                container.Register(implementationType.FullName, serviceType, implementationType);
            }
        };

        public static Action<INancyContainer, Type, Type> RegisterSingletonIfNoneHandler = (container, serviceType, implementationType) =>
        {
            if (!container.CanResolve(serviceType))
            {
                container.RegisterSingleton(implementationType.FullName, serviceType, implementationType);
            }
        };

        public RegistrationList Add<TService>()
        {
            Add<TService>(DefaultFilter);
            return this;
        }

        public RegistrationList Add<TService>(Func<Type, Type, bool> typeFilter)
        {
            Add<TService>(typeFilter, RegisterHandler);
            return this;
        }

        private RegistrationList Add<TService>(Func<Type, Type, bool> typeFilter, Action<INancyContainer, Type, Type> handler)
        {
            Add(new RegistrationData() { ServiceType = typeof(TService), Handler = handler, TypeFilter = typeFilter ?? DefaultFilter });
            return this;
        }

        public RegistrationList AddIfNone<TService>()
        {
            Add<TService>(DefaultFilter, RegisterIfNoneHandler);
            return this;
        }

        public RegistrationList AddIfNone<TService>(Func<Type, Type, bool> typeFilter)
        {
            Add<TService>(typeFilter, RegisterIfNoneHandler);
            return this;
        }

        public RegistrationList AddSingleton<TService>()
        {
            Add<TService>(DefaultFilter, RegisterSingletonHandler);
            return this;
        }

        public RegistrationList AddSingleton<TService>(Func<Type, Type, bool> typeFilter)
        {
            Add<TService>(typeFilter, RegisterSingletonHandler);
            return this;
        }

        public RegistrationList AddSingletonIfNone<TService>()
        {
            Add<TService>(DefaultFilter, RegisterSingletonIfNoneHandler);
            return this;
        }

        public RegistrationList AddSingletonIfNone<TService>(Func<Type, Type, bool> typeFilter)
        {
            Add<TService>(typeFilter, RegisterSingletonIfNoneHandler);
            return this;
        }
    }
}
