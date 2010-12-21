﻿namespace Nancy.Tests.Unit.Routing
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Nancy;
    using Nancy.Extensions;
    using Nancy.Routing;
    using Nancy.Tests.Fakes;
    using Xunit;
    using Xunit.Extensions;
    using Nancy.Configuration;

    public class RouteResolverFixture
    {
        private readonly IRouteResolver resolver;

        public RouteResolverFixture()
        {
            this.resolver = new RouteResolver();
        }

        [Fact]
        public void Should_return_no_matching_route_found_route_when_no_match_could_be_found()
        {
            // Given
            var request = new Request("GET", "/invalid", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());

            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };
            
            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.ShouldBeOfType<NoMatchingRouteFoundRoute>();
        }

        [Fact]
        public void Should_match_on_combination_of_module_base_path_and_action_path_when_module_defines_base_path()
        {
            // Given
            var request = new Request("GET", "/fake/route/with/some/parts", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());

            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.ShouldNotBeOfType<NoMatchingRouteFoundRoute>();
        }

        [Theory]
        [InlineData("/fake/Route/WITH/soMe/paRTs")]
        [InlineData("/FAKE/ROUTE/WITH/SOME/PARTS")]
        public void Should_be_case_insensitive_when_matching(string path)
        {
            // Given
            var request = new Request("GET", path, new Dictionary<string, IEnumerable<string>>(), new MemoryStream());

            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.ShouldNotBeOfType<NoMatchingRouteFoundRoute>();
        }

        [Fact]
        public void Should_not_match_on_combination_of_module_base_path_and_action_path_when_module_defines_base_path()
        {
            // Given
            var request = new Request("GET", "/route/with/some/parts", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());

            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.ShouldBeOfType<NoMatchingRouteFoundRoute>();
        }

        [Fact]
        public void Should_set_combination_of_module_base_path_and_action_path_on_no_matching_route_found_route_when_no_match_could_be_found()
        {
            // Given
            var request = new Request("GET", "/fake/route/with/some/parts", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.Path.ShouldEqual(request.Uri);
        }

        [Fact]
        public void Should_set_action_on_route_when_match_was_found()
        {
            // Given
            var request = new Request("GET", "/fake/route/with/some/parts", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());
            var response = route.Invoke();
            var output = response.GetStringContentsFromResponse();

            // Then
            output.ShouldEqual("FakeNancyModuleWithBasePath");
        }

        [Fact]
        public void Should_return_first_matched_route_when_conflicting_routs_are_available()
        {
            // Given
            var request = new Request("GET", "/fake/should/have/conflicting/route/defined", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());
            var response = route.Invoke();

            // When
            var output = response.GetStringContentsFromResponse();

            // Then
            output.ShouldEqual("FakeNancyModuleWithBasePath");
        }

        [Fact]
        public void Should_match_parameterized_action_path_with_request_path()
        {
            // Given
            var request = new Request("GET", "/fake/child/route", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.ShouldNotBeOfType<NoMatchingRouteFoundRoute>();
        }

        [Fact]
        public void Should_treat_action_route_parameters_as_greedy()
        {
            // Given
            var request = new Request("GET", "/fake/foo/some/stuff/not/in/route/bar/more/stuff/not/in/route", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.ShouldNotBeOfType<NoMatchingRouteFoundRoute>();
        }

        [Fact]
        public void Should_return_the_route_with_most_static_matches_when_multiple_matches_are_found()
        {
            // Given
            var request = new Request("GET", "/fake/child/route/foo", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());
            var response = route.Invoke();

            // When
            var output = response.GetStringContentsFromResponse();

            // Then
            output.ShouldEqual("test");
        }

        [Fact]
        public void Should_set_parameters_on_route_when_match_was_made_for_parameterized_action_route()
        {
            // Given
            var request = new Request("GET", "/fake/foo/some/stuff/not/in/route/bar/more/stuff/not/in/route", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };
            dynamic result;

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            Record.Exception(() => result = route.Parameters.value).ShouldBeNull();
            Record.Exception(() => result = route.Parameters.capture).ShouldBeNull();
        }

        [Fact]
        public void Should_pass_a_new_instance_of_the_module()
        {
            // Given
            var request = new Request("GET", "/fake/foo/some/stuff/not/in/route/bar/more/stuff/not/in/route", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };            

            // When
            var route1 = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());
            var route2 = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route1.Module.ShouldBeOfType(typeof(FakeNancyModuleWithBasePath));
            route2.Module.ShouldBeOfType(typeof(FakeNancyModuleWithBasePath));
            route1.Module.ShouldNotBeSameAs(route2.Module);
        }

        [Fact]
        public void Should_pass_the_application_to_the_module()
        {
            // Given
            var request = new Request("GET", "/fake/foo/some/stuff/not/in/route/bar/more/stuff/not/in/route", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };
            var application = NancyBootstrapper.BootstrapApplication();            

            // When
            var route = this.resolver.GetRoute(request, metas, application);

            // Then
            route.Module.Application.ShouldBeSameAs(application);
        }

        [Fact]
        public void Should_pass_the_request_to_the_module()
        {
            // Given
            var request = new Request("GET", "/fake/foo/some/stuff/not/in/route/bar/more/stuff/not/in/route", new Dictionary<string, IEnumerable<string>>(), new MemoryStream());
            var metas = new[] { new ModuleMeta(typeof(FakeNancyModuleWithBasePath), new FakeNancyModuleWithBasePath().GetRouteDescription("GET")) };                        

            // When
            var route = this.resolver.GetRoute(request, metas, NancyBootstrapper.BootstrapApplication());

            // Then
            route.Module.Request.ShouldBeSameAs(request);
        }

        protected static string GetStringContentsFromResponse(Response response)
        {
            var memory = new MemoryStream();
            response.Contents.Invoke(memory);
            memory.Position = 0;
            using (var reader = new StreamReader(memory))
            {
                return reader.ReadToEnd();
            }
        }
    }
}