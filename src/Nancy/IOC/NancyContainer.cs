namespace Nancy.IOC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.IOC.RegistrationAdapters;
    using Nancy.IOC.Registrations;

    public class NancyContainer : INancyContainer
    {
        private List<IRegistration> registrations = new List<IRegistration>();
        private List<IRegistrationAdapter> registrationAdapters = new List<IRegistrationAdapter>();

        public NancyContainer()
        {
            registrationAdapters.Add(new GenericFuncAdapter());
            registrationAdapters.Add(new CollectionAdapter());
            registrationAdapters.Add(new ConcreteAdapter());
            registrationAdapters.Add(new OpenGenericAdapter());
        }

        private IReuseScope reuseScope = ReuseScope.AlwaysNew;
        public void SetDefaultReuseScope(IReuseScope scope)
        {
            if (scope != null) reuseScope = scope;
        }

        public IRegistration Register(string name, Type serviceType, Func<INancyContainer, object> factory)
        {
            var reg = new FactoryRegistration(serviceType, factory, name).WithinScope(reuseScope);
            registrations.Add(reg);
            return reg;
        }

        public IRegistration Register(string name, Type serviceType, Type implementationType)
        {
            var reg = new TypeRegistration(name, serviceType, implementationType).WithinScope(reuseScope);
            registrations.Add(reg);
            return reg;
        }

        public IRegistration Register(string name, Type serviceType, object instance)
        {
            var reg = new InstanceRegistration(serviceType, instance, name).WithinScope(reuseScope);
            registrations.Add(reg);
            return reg;
        }

        public void RegisterSingleton(string name, Type serviceType, Func<INancyContainer, object> factory)
        {
            Register(name, serviceType, factory).WithinScope(ReuseScope.Singleton);
        }

        public void RegisterSingleton(string name, Type serviceType, Type implementationType)
        {
            Register(name, serviceType, implementationType).WithinScope(ReuseScope.Singleton);
        }

        public object Resolve(string name, Type serviceType)
        {
            var registration = GetRegistrations(serviceType, name).LastOrDefault();

            if (registration != null) {
                return registration.GetInstance(this);
            }

            return null;
        }

        public IEnumerable<IRegistration> GetRegistrations(Type serviceType, string name) {
            var regs = registrations.Where(r => r.ServiceType == serviceType);

            if (!string.IsNullOrEmpty(name))
            {
                regs = regs.Where(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            if (regs != null && regs.Count() > 0)
            {
                return regs;
            }

            foreach (var adapter in registrationAdapters)
            {
                if (adapter.CanCreateRegistrationsFor(serviceType))
                {
                    var dynamicregs = adapter.GetRegistrationsFor(this, serviceType);
                    if (dynamicregs != null && dynamicregs.Count() > 0)
                    {
                        registrations.AddRange(dynamicregs);
                        return GetRegistrations(serviceType, name);
                    }
                }
                
            }
            
            return Enumerable.Empty<IRegistration>();
        }

        
        public void Dispose()
        {
            
        }

        public bool CanResolve(Type serviceType)
        {
            return (registrations.Any(r => r.ServiceType == serviceType));
        }

        

        object INancyContainer.Resolve(string name, Type serviceType)
        {
            return Resolve(name, serviceType);
        }

        void INancyContainer.Register(string name, Type serviceType, object instance)
        {
            Register(name, serviceType, instance);
        }

        void INancyContainer.Register(string name, Type serviceType, Func<INancyContainer, object> factory)
        {
            Register(name, serviceType, factory);   
        }

        void INancyContainer.Register(string name, Type serviceType, Type implementationType)
        {
            Register(name, serviceType, implementationType);
        }
    }
}
