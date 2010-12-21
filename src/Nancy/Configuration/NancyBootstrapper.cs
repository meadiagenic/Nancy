namespace Nancy.Configuration
{
    using System;
    using Nancy.IOC;
    using System.Linq;
    using System.Collections.Generic;
    using Nancy.Routing;

    public abstract class NancyBootstrapper
    {
        TypeFinder _finder = new TypeFinder();
        
        public void UseContainer(INancyContainer container)
        {
            Container = container;
        }

        private INancyContainer _container;
        public INancyContainer Container
        {
            get
            {
                return _container = _container ?? new NancyContainer();
            }
            private set
            {
                _container = value;
            }
        }

        private RegistrationList _registrations;
        public RegistrationList Registrations
        {
            get { return _registrations = _registrations ?? new RegistrationList(); }
        }

        public AssemblyLoader ForAssemblies
        {
            get { return new AssemblyLoader(_finder); }
        }

        public INancyApplication Bootstrap()
        {
            var container = Container;

            var registrationList = Registrations;
            registrationList.Add<INancyRegistrar>();
            registrationList.Add<INancyComponent>();
            ProcessRegistrationList(registrationList, _finder);
            
            var localRegistrations = new RegistrationList();
            localRegistrations.Add<INancyRegistrar>();
            localRegistrations.Add<INancyComponent>();
            ProcessRegistrationList(localRegistrations, new TypeFinder(typeof(NancyBootstrapper).Assembly));

            var registrations = container.Resolve<IEnumerable<INancyRegistrar>>();

            if (registrations != null && registrations.Count() > 0)
            {
                foreach (var registration in registrations)
                {
                    registration.Register(container);
                }
            }

            container.RegisterIfNone<INancyContainer>(container);
            container.RegisterIfNone<INancyApplication, NancyApplication>();
            return container.Resolve<INancyApplication>();
        }

        private void ProcessRegistrationList(RegistrationList registrationList, TypeFinder finder)
        {
            foreach (var registration in registrationList)
            {
                var serviceType = registration.ServiceType;
                var handler = registration.Handler;
                var typeFilter = registration.TypeFilter;

                var registrationTypes = finder.Types.Where(type => typeFilter(type, serviceType));

                foreach (Type type in registrationTypes)
                {
                    handler(this.Container, type);
                }
            }
        }
    }
}
