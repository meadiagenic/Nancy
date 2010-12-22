namespace Nancy.Configuration
{
    using System;
    using Nancy.IOC;
    using System.Collections.Generic;
    using Nancy.Extensions;

    public class NancyModuleComponent : INancyComponent
    {
        public void AddRegistrations(RegistrationList registrations)
        {
            registrations.Add<NancyModule>();
        }

        public void Initialize(INancyApplication application)
        {
            var modules = application.Container.Resolve<IEnumerable<Func<NancyModule>>>();
            foreach (var module in modules)
            {
                var mod = module();
                AddModuleMetaToApplication(application, mod, "GET");

                AddModuleMetaToApplication(application, mod, "POST");

                AddModuleMetaToApplication(application, mod, "PUT");

                AddModuleMetaToApplication(application, mod, "DELETE");
            }
        }

        private static void AddModuleMetaToApplication(INancyApplication application, NancyModule mod, string action)
        {
            IList<ModuleMeta> postList = GetModuleMetaList(application, action);
            postList.Add(new ModuleMeta(mod.GetType(), mod.GetRouteDescription(action)));
        }

        private static IList<ModuleMeta> GetModuleMetaList(INancyApplication application, string method)
        {
            IEnumerable<ModuleMeta> list;
            if (!application.ModuleMetas.TryGetValue(method, out list))
            {
                list = new List<ModuleMeta>();
                application.ModuleMetas[method] = list;
            }
            return list as IList<ModuleMeta>;
        }

        public void Run(INancyApplication application)
        {
           
        }
    }
}
