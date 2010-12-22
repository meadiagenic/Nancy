using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC.Registrations;

namespace Nancy.IOC.RegistrationAdapters
{
    public class CollectionAdapter : IRegistrationAdapter
    {
        public IEnumerable<IRegistration> GetRegistrationsFor(NancyContainer container, Type serviceType)
        {
            if (serviceType != null)
            {
                Type elementType = null;
                Func<INancyContainer, object> delegateActivator = null;
                if (serviceType.IsGenericTypeDefinedBy(typeof(IEnumerable<>)))
                {
                    elementType = serviceType.GetGenericArguments()[0];           
                }
                else if (serviceType.IsArray)
                {
                    elementType = serviceType.GetElementType();
                }

                if (elementType != null)
                {
                    var elementArrayType = elementType.MakeArrayType();

                    delegateActivator = (c) =>
                    {
                        var nc = c as NancyContainer;
                        if (nc != null)
                        {
                            var registrations = nc.GetRegistrations(elementType, null).ToArray();
                            var elements = registrations.Select(r => r.GetInstance(nc)).ToArray();
                            var result = Array.CreateInstance(elementType, elements.Length);
                            elements.CopyTo(result, 0);
                            return result;
                        }

                        return null;
                    };

                    return new IRegistration[] { new FactoryRegistration(serviceType, delegateActivator, null) };
                }

                if (delegateActivator != null) return new IRegistration[] { new FactoryRegistration(serviceType, delegateActivator, null) };
            }

            return Enumerable.Empty<IRegistration>();
        }

        public bool CanCreateRegistrationsFor(Type serviceType)
        {
            return (serviceType.IsGenericTypeDefinedBy(typeof(IEnumerable<>))
                || serviceType.IsArray);
        }
    }
}
