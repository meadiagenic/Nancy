using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.IOC.Registrations
{
    public class InstanceRegistration : BaseRegistration
    {
        public InstanceRegistration(Type serviceType, object instance, string name)
            : base(serviceType, name)
        {
            Instance = instance;
        }

        public override object GetInstance(NancyContainer container)
        {
            return Instance;
        }

        public override object CreateInstance(NancyContainer container)
        {
            return Instance;
        }
    }
}
