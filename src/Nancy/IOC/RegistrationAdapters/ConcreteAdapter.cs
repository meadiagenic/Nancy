namespace Nancy.IOC.RegistrationAdapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
using Nancy.IOC.Registrations;

    public class ConcreteAdapter : IRegistrationAdapter
    {
        public bool CanCreateRegistrationsFor(Type serviceType)
        {
            return (serviceType != null
                && !serviceType.IsAbstract
                && serviceType.IsClass 
                && !serviceType.IsSubclassOf(typeof(Delegate)));
        }

        public IEnumerable<IRegistration> GetRegistrationsFor(NancyContainer container, Type serviceType)
        {
            if (CanCreateRegistrationsFor(serviceType))
            {
                return new IRegistration[] { new TypeRegistration(string.Empty, serviceType, serviceType) };
            }
            return Enumerable.Empty<IRegistration>();
        }
    }
}
