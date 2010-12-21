namespace Nancy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using ViewEngines;
    using Nancy.IOC;

    public class NancyApplication : INancyApplication
    {
        private IDictionary<string, Func<string, object, Action<Stream>>> templateProcessors;
        private IDictionary<string, IEnumerable<ModuleMeta>> modules;

        public NancyApplication(INancyContainer container)
        {
            this.Container = container;
            
        }

        public IDictionary<string, Func<string, object, Action<Stream>>> TemplateProcessors
        {
            get { return templateProcessors = templateProcessors ?? new Dictionary<string, Func<string, object, Action<Stream>>>(StringComparer.CurrentCultureIgnoreCase); }
        }

        public IDictionary<string, IEnumerable<ModuleMeta>> ModuleMetas
        {
            get { return modules = modules ?? new Dictionary<string, IEnumerable<ModuleMeta>>(StringComparer.CurrentCultureIgnoreCase); }
        }

        public Func<string, object, Action<Stream>> GetTemplateProcessor(string extension)
        {
            return this.templateProcessors.ContainsKey(extension) ? this.templateProcessors[extension] : null;
        }

        public Func<string, object, Action<Stream>> DefaultProcessor
        {
            get { return (path, model) => StaticViewEngineExtension.Static(null, path); }
        }

        public INancyContainer Container
        {
            get;
            private set;
        }

        //private IDictionary<string, IEnumerable<ModuleMeta>> LoadModules(IEnumerable<Type> allTypes)
        //{
        //    var types = from type in allTypes
        //                where activator.CanCreateInstance(type)
        //                select type;

        //    var metas = new Dictionary<string, IEnumerable<ModuleMeta>>(StringComparer.CurrentCultureIgnoreCase)
        //                {
        //                    {"GET", new List<ModuleMeta>(types.Count())},
        //                    {"POST", new List<ModuleMeta>(types.Count())},
        //                    {"PUT", new List<ModuleMeta>(types.Count())},
        //                    {"DELETE", new List<ModuleMeta>(types.Count())},
        //                };
        //    foreach (var type in types)
        //    {
        //        var module = Activator.CreateInstance(type);
        //        ((List<ModuleMeta>)metas["GET"]).Add(new ModuleMeta(type, module.GetRouteDescription("GET")));
        //        ((List<ModuleMeta>)metas["POST"]).Add(new ModuleMeta(type, module.GetRouteDescription("POST")));
        //        ((List<ModuleMeta>)metas["PUT"]).Add(new ModuleMeta(type, module.GetRouteDescription("PUT")));
        //        ((List<ModuleMeta>)metas["DELETE"]).Add(new ModuleMeta(type, module.GetRouteDescription("DELETE")));

        //    }
        //    return metas;
        //}

        public INancyEngine GetEngine()
        {
            return Container.Resolve<INancyEngine>();
        }
    }
}