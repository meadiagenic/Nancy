namespace Nancy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

        public NancyApplication(INancyContainer container)
        {
            Container = container;
            this.templateProcessors = LoadTemplates();
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

        private IDictionary<string, Func<string, object, Action<Stream>>> LoadTemplates()
        {
            var registries = Container.Resolve<IEnumerable<IViewEngineRegistry>>();
            
            var templates = new Dictionary<string, Func<string, object, Action<Stream>>>(registries.Count(), StringComparer.CurrentCultureIgnoreCase);
            foreach (var registry in registries)
            {
                templates.Add(registry.Extension, registry.Executor);
            }
            return templates;
        }
    }
}