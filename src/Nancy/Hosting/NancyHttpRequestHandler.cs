namespace Nancy.Hosting
{
    using System.Web;
    using Routing;
    using Extensions;
    using Nancy.Configuration;

    public class NancyHttpRequestHandler : IHttpHandler
    {
        private readonly static INancyApplication application = NancyBootstrapper.BootstrapApplication();

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var engine = application.GetEngine();
            var httpContext = new HttpContextWrapper(context);
            var request = CreateNancyRequestFromHttpContext(httpContext);
            //var engine = new NancyEngine(
            //    CreateModuleLocator(),
            //    new RouteResolver(),
            //    application);

            var response = engine.HandleRequest(request);

            SetNancyResponseToHttpResponse(httpContext, response);
        }

        private IRequest CreateNancyRequestFromHttpContext(HttpContextBase context)
        {
            return new Request(
                context.Request.HttpMethod,
                context.Request.Url.AbsolutePath,
                context.Request.Headers.ToDictionary(),
                context.Request.InputStream);
        }

        private void SetNancyResponseToHttpResponse(HttpContextBase context, Response response)
        {
            SetHttpResponseHeaders(context, response);

            context.Response.ContentType = response.ContentType;
            context.Response.StatusCode = (int)response.StatusCode;
            response.Contents.Invoke(context.Response.OutputStream);
        }

        private static void SetHttpResponseHeaders(HttpContextBase context, Response response)
        {
            foreach (var pair in response.Headers)
            {
                context.Response.Headers.Add(pair.Key, pair.Value);
            }
        }
        //protected virtual INancyModuleLocator CreateModuleLocator()
        //{
        //    return new AppDomainModuleLocator(new DefaultModuleActivator());
        //}
    }
}