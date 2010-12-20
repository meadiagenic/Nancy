namespace Nancy.Hosting
{
    using System.Web;
    using Routing;

    public class NancyHttpRequestHandler : IHttpHandler
    {
        private readonly static INancyApplication application = new NancyApplication();

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var engine = new NancyEngine(
                CreateModuleLocator(),
                new RouteResolver(),
                application);

            var wrappedContext = new HttpContextWrapper(context);
            var handler = new NancyHandler(engine);
            handler.ProcessRequest(wrappedContext);
        }

        protected virtual INancyModuleLocator CreateModuleLocator()
        {
            return new AppDomainModuleLocator(new DefaultModuleActivator());
        }
    }
}