namespace Nancy.ViewEngines.NDjango
{
   using System;
   using System.IO;

    public class NDjangoViewRegistry : IViewEngineRegistry
    {
        public NDjangoViewRegistry(Func<NDjangoViewEngine> engine)
        {
            Engine = engine;
        }

        public Func<NDjangoViewEngine> Engine { get; private set; }
        public string Extension
        {
            get { return ".django"; }
        }

        public Func<string, object, Action<Stream>> Executor
        {
            get
            {
                return (name, model) =>
                {
                    return stream =>
                    {
                        var result = Engine().RenderView(name, model);
                        result.Execute(stream);
                    };
                };
            }
        }
    }
}