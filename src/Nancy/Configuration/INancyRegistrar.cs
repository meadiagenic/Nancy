using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC;

namespace Nancy.Configuration
{
    public interface INancyRegistrar
    {
        void Register(INancyContainer container);
    }
}
