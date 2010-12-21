namespace Nancy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;    
    using Extensions;
    using ViewEngines;
    using Configuration;
    using Extensions;
    using Nancy.IOC;

    public class NancyApplication : INancyApplication
    {
        private static NancyBootstrapper _bootstrapper;
        public static NancyBootstrapper BootStrapper
        {
            get { return _bootstrapper = _bootstrapper ?? GetBootstrapper(); }
        }
        
        private static NancyBootstrapper GetBootstrapper()
        {
            var tf = new TypeFinder();
            var types = tf.TypesImplementing<NancyBootstrapper>().ConcreteClasses().CanCreateInstance();
            var bootstrapperType = types.FirstOrDefault() ?? typeof(DefaultBootstrapper);
            return Activator.CreateInstance(bootstrapperType) as NancyBootstrapper;   
        }

        public static void UseBootstrapper<T>() where T : NancyBootstrapper, new()
        {
            _bootstrapper = Activator.CreateInstance<T>();
        }

        public static void BootstrapWith(Action<NancyBootstrapper> bootstrapper)
        {
            var strapper = new EmptyBootstrapper();
            bootstrapper(strapper);
            _bootstrapper = strapper;
        }

        public static INancyApplication Bootstrap()
        {
            return BootStrapper.Bootstrap();
        }

        private readonly IDictionary<string, Func<string, object, Action<Stream>>> templateProcessors;
        private readonly IDictionary<string, IEnumerable<ModuleMeta>> modules;
        private readonly IModuleActivator activator;

        public NancyApplication(INancyContainer container)
        {
            Container = container;
            this.activator = container.Resolve<IModuleActivator>;
            var types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                         where !type.IsAbstract
                         select type).ToList();

            this.templateProcessors = LoadTemplates(types);
            this.modules = LoadModules(types);
        }

        public INancyContainer Container
        {
            get;
            private set;
        }

        public INancyEngine GetEngine()
        {
            return Container.Resolve<INancyEngine>();
        }

        public Func<string, object, Action<Stream>> GetTemplateProcessor(string extension)
        {
            return this.templateProcessors.ContainsKey(extension) ? this.templateProcessors[extension] : null;
        }

        public Func<string, object, Action<Stream>> DefaultProcessor
        {
            get { return (path, model) => StaticViewEngineExtension.Static(null, path); }
        }

        public IModuleActivator Activator
        {
            get { return this.activator; }
        }

        public IDictionary<string, IEnumerable<ModuleMeta>> GetModules()
        {
            return this.modules;
        }


        private IDictionary<string, IEnumerable<ModuleMeta>> LoadModules(IEnumerable<Type> allTypes)
        {
            var types = from type in allTypes                                                
                        where activator.CanCreateInstance(type)
                        select type;

            var metas = new Dictionary<string, IEnumerable<ModuleMeta>>(StringComparer.CurrentCultureIgnoreCase)
                        {
                            {"GET", new List<ModuleMeta>(types.Count())},
                            {"POST", new List<ModuleMeta>(types.Count())},
                            {"PUT", new List<ModuleMeta>(types.Count())},
                            {"DELETE", new List<ModuleMeta>(types.Count())},
                        };
            foreach (var type in types)
            {
                var module = Activator.CreateInstance(type);
                ((List<ModuleMeta>)metas["GET"]).Add(new ModuleMeta(type, module.GetRouteDescription("GET")));
                ((List<ModuleMeta>)metas["POST"]).Add(new ModuleMeta(type, module.GetRouteDescription("POST")));
                ((List<ModuleMeta>)metas["PUT"]).Add(new ModuleMeta(type, module.GetRouteDescription("PUT")));
                ((List<ModuleMeta>)metas["DELETE"]).Add(new ModuleMeta(type, module.GetRouteDescription("DELETE")));

            }
            return metas;
        }

        private static IDictionary<string, Func<string, object, Action<Stream>>> LoadTemplates(IEnumerable<Type> types)
        {
            var registries = from type in types
                             where typeof (IViewEngineRegistry).IsAssignableFrom(type)
                             select type;
            var templates = new Dictionary<string, Func<string, object, Action<Stream>>>(registries.Count(), StringComparer.CurrentCultureIgnoreCase);
            foreach (var registry in registries)
            {
                templates.Add(registry.Extension, registry.Executor);
            }
            return templates;
        }
    }
}