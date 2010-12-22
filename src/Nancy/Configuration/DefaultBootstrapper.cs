namespace Nancy.Configuration
{
    using System;

    public class DefaultBootstrapper : NancyBootstrapper
    {
        public DefaultBootstrapper()
        {
            ForAssemblies.InBinFolder().ReferencedAssemblies();
        }
    }
}
