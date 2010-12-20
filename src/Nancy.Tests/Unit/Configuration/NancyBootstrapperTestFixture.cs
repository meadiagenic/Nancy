namespace Nancy.Tests.Unit.Configuration
{
    using System;
using Xunit;
    using Nancy.Configuration;
    using Nancy.IOC;

    public class NancyBootstrapperTestFixture
    {
        [Fact]
        public void BaseNancyBootstrapper_Uses_NancyContainer()
        {
            var container = new TestBootStrapper().Container;

            container.ShouldNotBeNull();
            container.ShouldBeOfType<NancyContainer>();
        }

        [Fact]
        public void BaseNancyBootstrapper_Automatically_loads_INancyEngine()
        {
            var bootstrapper = new TestBootStrapper();

            var application = bootstrapper.Bootstrap();

            var engine = application.GetEngine();

            engine.ShouldNotBeNull();
            engine.ShouldBeOfType<NancyEngine>();
        }
    }

    public class TestBootStrapper : NancyBootstrapper
    {
    }
}
