using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.Configuration
{
    public interface INancyComponent
    {
        void AddRegistrations(RegistrationList registrations);

        void Initialize(INancyApplication application);

        void Run(INancyApplication application);
    }
}
