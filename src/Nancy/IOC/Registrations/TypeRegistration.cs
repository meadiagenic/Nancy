namespace Nancy.IOC.Registrations
{
    using System;

    public class TypeRegistration : BaseRegistration
    {
        public TypeRegistration(string name, Type serviceType, Type implementationType) : base(serviceType, name)
        {
            if (implementationType == null) throw new ArgumentNullException("implementationType");
            ImplementationType = implementationType;
        }

        public Type ImplementationType { get; private set; }

        private Func<INancyContainer, object> Factory { get; set; }

        public override object CreateInstance(NancyContainer container)
        {
            if (Factory == null)
            {
                Factory = ImplementationType.CreateFactoryDelegate<object>(container);
            }
            return Factory(container);
        }
    }
}
