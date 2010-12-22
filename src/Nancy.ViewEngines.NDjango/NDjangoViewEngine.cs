namespace Nancy.ViewEngines.NDjango
{
    using System.Collections.Generic;
    using System.IO;
    using global::NDjango;
    using global::NDjango.Interfaces;

    public class NDjangoViewEngine
    {
        public NDjangoViewEngine(IViewLocator viewTemplateLocator, TemplateManagerProvider provider)
        {
            ViewTemplateLocator = viewTemplateLocator;
            TemplateManagerProvider = provider;
        }

        public TemplateManagerProvider TemplateManagerProvider { get; private set; }
        public IViewLocator ViewTemplateLocator { get; private set; }

        public ViewResult RenderView<TModel>(string viewTemplate, TModel model)
        {
            var manager = TemplateManagerProvider.GetNewManager();

            var result = ViewTemplateLocator.GetTemplateContents(viewTemplate);

            var context = new Dictionary<string, object> { { "Model", model } };

            string location = result.Location;

            TextReader reader = manager.RenderTemplate(location, context);

            var view = new NDjangoView(reader) {Model = model};

            return new ViewResult(view, location);
        }       
    }
}