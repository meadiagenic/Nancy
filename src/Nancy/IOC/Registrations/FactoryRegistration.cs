namespace Nancy.IOC.Registrations
{
    using System;

    public class FactoryRegistration : BaseRegistration
    {
        public FactoryRegistration(Type serviceType, Func<INancyContainer, object> factory, string name) : base(serviceType, name)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            Factory = factory;
        }

        public Func<INancyContainer, object> Factory { get; private set; }

        public override object CreateInstance(NancyContainer container)
        {
            return Factory(container);
        }
    }
}
