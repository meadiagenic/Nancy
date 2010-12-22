namespace Nancy.Configuration
{
    using System;
    using Nancy.IOC;
    using System.Linq;
    using System.Collections.Generic;
    using Nancy.Routing;
    using Nancy.Extensions;

    public abstract class NancyBootstrapper
    {
        public static INancyApplication BootstrapApplication()
        {
            var typeFinder = new TypeFinder();
            var bootStrapperType = typeFinder
                .TypesImplementing<NancyBootstrapper>()
                .ConcreteClasses()
                .CanCreateInstance()
                .FirstOrDefault() ?? typeof(DefaultBootstrapper);
            var bootStrapper = Activator.CreateInstance(bootStrapperType) as NancyBootstrapper;

            if (bootStrapper != null) return bootStrapper.Bootstrap();
            throw new InvalidOperationException("Can't find NancyBootstrapper.");
        }

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
            
            _finder.AddAssembly(typeof(NancyBootstrapper).Assembly);
            ProcessRegistrationList(registrationList, _finder);

            var registrations = container.Resolve<IEnumerable<INancyRegistrar>>();

            if (registrations != null && registrations.Count() > 0)
            {
                foreach (var registration in registrations)
                {
                    registration.Register(container);
                }
            }

            var componentRegistrations = new RegistrationList();
            var components = container.Resolve<IEnumerable<INancyComponent>>();
            foreach (var component in components)
            {
                component.AddRegistrations(componentRegistrations);
            }
            ProcessRegistrationList(componentRegistrations, _finder);

            container.RegisterIfNone<INancyContainer>(container);

            var application = container.Resolve<INancyApplication>();
            foreach (var component in components)
            {
                component.Initialize(application);
            }
            return application;
        }

        private void ProcessRegistrationList(RegistrationList registrationList, TypeFinder finder)
        {
            foreach (var registration in registrationList)
            {
                var serviceType = registration.ServiceType;
                var handler = registration.Handler;
                var typeFilter = registration.TypeFilter;

                var registrationTypes = finder.Types.Where(type => typeFilter(type, serviceType)).AsEnumerable();

                foreach (Type type in registrationTypes)
                {
                    handler(this.Container, serviceType, type);
                }
            }
        }
    }
}
