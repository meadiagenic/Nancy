namespace Nancy.Configuration
{
    using System;
    using System.Collections.Generic;
    using Nancy.IOC;
    using Nancy.ViewEngines;

    public class ViewEngineRegistryComponent : INancyComponent
    {
        public void AddRegistrations(RegistrationList registrations)
        {
            registrations.Add<IViewEngineRegistry>();
            registrations.Add<IViewLocator>();
        }

        public void Initialize(INancyApplication application)
        {
            var registries = application.Container.Resolve<IEnumerable<IViewEngineRegistry>>();
            foreach (var registry in registries)
            {
                application.TemplateProcessors.Add(registry.Extension, registry.Executor);
            }
        }

        public void Run(INancyApplication application)
        {
            
        }
    }
}
