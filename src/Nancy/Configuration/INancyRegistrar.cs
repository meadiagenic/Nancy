namespace Nancy.Configuration
{
    using System;

    public interface INancyRegistrar
    {
        Type ServiceType { get; }
    }
}
