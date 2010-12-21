using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC;

namespace Nancy.Configuration
{
    public class RegistrationList : List<RegistrationData>
    {
        private static readonly Func<Type, Type, bool> defaultFilter = (serviceType, registrationType) =>
        {
            return registrationType.IsAssignableFrom(serviceType) &&
                serviceType != registrationType &&
                !serviceType.IsAbstract &&
                !serviceType.IsGenericTypeDefinition &&
                !serviceType.ContainsGenericParameters;
        };

        public RegistrationList Add<TService>()
        {
            Add<TService>(defaultFilter);
            return this;
        }

        public RegistrationList Add<TService>(Func<Type, Type, bool> typeFilter)
        {
            Add<TService>(typeFilter, (container, type) => container.Register<TService>(type));
            return this;
        }

        public RegistrationList Add<TService>(Func<Type, Type, bool> typeFilter, Action<INancyContainer, Type> handler)
        {
            Add(new RegistrationData() { ServiceType = typeof(TService), Handler = handler, TypeFilter = typeFilter });
            return this;
        }
    }
}
