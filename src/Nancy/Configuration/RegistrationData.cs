using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC;

namespace Nancy.Configuration
{
    public class RegistrationData
    {
        public Type ServiceType { get; set; }
        public Action<INancyContainer, Type> Handler { get; set; }
        public Func<Type, Type, bool> TypeFilter { get; set; }
    }
}
