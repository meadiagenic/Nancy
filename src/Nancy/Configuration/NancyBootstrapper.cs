namespace Nancy.Configuration
{
    using System;
    using Nancy.IOC;
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
                return _container ?? new NancyContainer();
            }
            private set
            {
                _container = value;
            }
        }

        public AssemblyLoader ForAssemblies
        {
            get { return new AssemblyLoader(_finder); }
        }

        public INancyApplication Bootstrap()
        {
            var container = Container;

            if (!container.Contains<INancyEngine>())
            {
                container.RegisterIfNone<INancyModuleLocator, AppDomainModuleLocator>();
                container.RegisterIfNone<IModuleActivator, DefaultModuleActivator>();
                container.RegisterIfNone<IRouteResolver, RouteResolver>();
                container.RegisterIfNone<INancyEngine, NancyEngine>();
            }
            container.RegisterIfNone<INancyContainer>(container);
            container.RegisterIfNone<INancyApplication, NancyApplication>();
            return container.Resolve<INancyApplication>();
        }
    }
}
