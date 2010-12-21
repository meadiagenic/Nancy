using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC;
using Nancy.Routing;

namespace Nancy.Configuration
{
    public class NancyEngineRegistrar : INancyRegistrar
    {
        public void Register(INancyContainer container)
        {
            if (!container.Contains<INancyEngine>())
            {
                container.RegisterIfNone<IModuleActivator, DefaultModuleActivator>();
                container.RegisterIfNone<IRouteResolver, RouteResolver>();
                container.RegisterIfNone<INancyEngine, NancyEngine>();
            }
        }
    }
}
