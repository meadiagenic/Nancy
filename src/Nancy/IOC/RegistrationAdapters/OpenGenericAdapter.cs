namespace Nancy.IOC.RegistrationAdapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.IOC.Registrations;

    public class OpenGenericAdapter : IRegistrationAdapter
    {
        public bool CanCreateRegistrationsFor(Type serviceType)
        {
            return (serviceType != null && serviceType.IsInterface && serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition);
        }

        public IEnumerable<IRegistration> GetRegistrationsFor(NancyContainer container, Type serviceType)
        {
            if (CanCreateRegistrationsFor(serviceType))
            {
                Type genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                Type[] genericArguments = serviceType.GetGenericArguments();

                var newRegistrations = new List<IRegistration>();
                foreach (var registration in container.GetRegistrations(genericTypeDefinition, null))
                {
                    var typeRegistration = registration as TypeRegistration;
                    if (typeRegistration != null)
                    {
                        var implType = typeRegistration.ImplementationType.MakeGenericType(genericArguments);
                        newRegistrations.Add(new TypeRegistration(registration.Name, serviceType, implType));
                    }
                }
                return newRegistrations.AsEnumerable();
            }

            return Enumerable.Empty<IRegistration>();
        }
    }
}
