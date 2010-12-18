namespace Nancy.IOC.Registrations
{
    using System;

    public abstract class BaseRegistration : IRegistration, IInstanceCreator
    {
        protected BaseRegistration(Type serviceType, string name)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            ServiceType = serviceType;
            Name = name ?? string.Empty;
        }
        public string Name { get; private set; }

        public Type ServiceType { get; private set; }

        public IReuseScope Scope { get; private set; }

        public object Instance { get; set; }

        public virtual object GetInstance(NancyContainer container)
        {
            if (Scope != null)
            {
                return Scope.GetInstance(container, this);
            }
            return CreateInstance(container);
        }

        public IRegistration WithinScope(IReuseScope scope)
        {
            if (scope != null) Scope = scope;
            return this;
        }

        public abstract object CreateInstance(NancyContainer container);

    }
}
