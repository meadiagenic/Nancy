namespace Nancy.Configuration
{
    using System;
    using Nancy.IOC;
    using System.Collections.Generic;

    public abstract class NancyBootstapper
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


        public void Bootstrap()
        {
            var container = Container;

            foreach (var registrar in Registrars)
            {
                
            }
        }
    }
}
