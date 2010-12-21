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

        [Fact]
        public void BootStrapper_Can_Auto_Resolve_New_Registrations()
        {
            var bootstrapper = new TestBootStrapper();
            bootstrapper.Registrations.Add<IBootStrapperFoo>();

            var application = bootstrapper.Bootstrap();
            var foo = application.Container.Resolve<IBootStrapperFoo>();
            foo.ShouldNotBeNull();
            foo.ShouldBeOfType<TestBootStrapperFoo>();
        }

        [Fact]
        public void BootStrapper_Will_Load_Components_From_Nancy_Core()
        {
            var bootstrapper = new TestBootStrapper().Bootstrap();

            var foo = bootstrapper.Container.Resolve<INancyEngine>();
            foo.ShouldNotBeNull();
            foo.ShouldBeOfType<NancyEngine>();
        }
    }

    public class TestBootStrapper : NancyBootstrapper
    {
    }

    public interface IBootStrapperFoo
    {
    }

    public class TestBootStrapperFoo : IBootStrapperFoo
    {
    }
}
