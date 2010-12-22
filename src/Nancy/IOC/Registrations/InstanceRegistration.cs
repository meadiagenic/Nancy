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

        private WeakReference instanceWrapper;
        public override object Instance
        {
            get
            {
                if (instanceWrapper != null && instanceWrapper.IsAlive)
                {
                    return instanceWrapper.Target;
                }
                return null;
            }
            set
            {
                instanceWrapper = new WeakReference(value);
            }
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
