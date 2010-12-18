namespace Nancy.IOC
{
    using System;
    using System.Collections.Generic;

    public interface IRegistrationAdapter
    {
        bool CanCreateRegistrationsFor(Type serviceType);
        IEnumerable<IRegistration> GetRegistrationsFor(NancyContainer container, Type serviceType); 
    }
}
