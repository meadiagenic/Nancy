namespace Nancy.ViewEngines.NDjango
{
    using System;
    using Nancy.Configuration;
    using Nancy.IOC;
    using global::NDjango;
    using global::NDjango.Interfaces;
    
    using System.Collections.Generic;

    public class NDjangoNancyComponent : INancyComponent, INancyRegistrar
    {
        public void AddRegistrations(RegistrationList registrations)
        {
            registrations
                .Add<Tag>()
                .Add<Filter>()
                .Add<Setting>();
            
        }

        public void Initialize(INancyApplication application)
        {
            var container = application.Container;

            if (!container.Contains<TemplateManagerProvider>())
            {
                var tags = container.Resolve<IEnumerable<Tag>>();
                var filters = container.Resolve<IEnumerable<Filter>>();
                var settings = container.Resolve<IEnumerable<Setting>>();
                var templateProvider = new TemplateManagerProvider()
                    .WithFilters(filters)
                    .WithTags(tags)
                    .WithSettings(settings);
                container.Register<TemplateManagerProvider>(templateProvider);
            }
        }

        public void Run(INancyApplication application)
        {
        }

        public void Register(INancyContainer container)
        {
            container.Register<NDjangoViewEngine, NDjangoViewEngine>();
            if (!container.Contains<TemplateManagerProvider>())
            {
                container.RegisterSingleton<TemplateManagerProvider>(c =>
                {
                    return new TemplateManagerProvider()
                    .WithFilters(c.Resolve<IEnumerable<Filter>>())
                    .WithTags(c.Resolve<IEnumerable<Tag>>())
                    .WithSettings(c.Resolve<IEnumerable<Setting>>());
                });
            }
        }
    }
}
