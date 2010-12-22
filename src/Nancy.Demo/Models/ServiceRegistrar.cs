using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Configuration;
using Nancy.IOC;

namespace Nancy.Demo.Models
{
    public class ServiceRegistrar : INancyRegistrar
    {

        public void Register(INancyContainer container)
        {
            container.Register<IPackService, DefaultPackService>();
        }
    }
}