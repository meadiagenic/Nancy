namespace Nancy.IOC
{
    using System;

    public interface IRegistration
    {
        string Name { get; }
        Type ServiceType { get; }

        IRegistration WithinScope(IReuseScope scope);

        object GetInstance(NancyContainer container);
    }
}
