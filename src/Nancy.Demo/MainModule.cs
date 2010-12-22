namespace Nancy.Demo
{
    using Nancy.Demo.Models;
    using Nancy.Formatters;
    using Nancy.ViewEngines;
    using Nancy.ViewEngines.NDjango;
    using Nancy.ViewEngines.NHaml;
    using Nancy.ViewEngines.Razor;
    using System;

    public class Module : NancyModule
    {
        public Module(Func<IPackService> packService)
        {
            Get["/"] = x => {
                return "This is the root. Visit <a href=\"/static\">/static</a>, <a href=\"/razor\">/razor</a>, <a href=\"/nhaml\">/nhaml</a> or <a href=\"/ndjango\">/ndjango!</a>";
            };
            
            Get["/test"] = x => {
                return "Test";
            };

            Get["/static"] = x => {
                return View.Static("~/views/static.htm");
            };

            Get["/razor"] = x => {
                var model = packService().GetPackMember("Frank");
                return View.Razor("~/views/razor.cshtml", model);
            };
            Get["/nhaml"] = x => {
                //var model = new RatPack { FirstName = "Andrew" };
                var model = packService().GetPackMember("Dean");
                return View.Haml("~/views/nhaml.haml", model);
            };

            Get["/ndjango"] = x => {
                //var model = new RatPack { FirstName = "Michael" };
                var model = packService().GetPackMember("Sammy");
                return View.Django("~/views/ndjango.django", model);
			};

            Get["/json"] = x => {
                //var model = new RatPack { FirstName = "Frank" };
                var model = packService().GetPackMember("Frank");
                return Response.AsJson(model);
            };

            Get["/xml"] = x => {
                //var model = new RatPack { FirstName = "Frank" };
                var model = packService().GetPackMember("Frank");
                return Response.AsXml(model);
            };
        }
    }
}
