using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC;

namespace Nancy.Configuration
{
    public class NancyApplicationRegistrar : INancyRegistrar
    {
        public void Register(INancyContainer container)
        {
            container.RegisterSingletonIfNone<INancyApplication, NancyApplication>();
        }
    }
}
