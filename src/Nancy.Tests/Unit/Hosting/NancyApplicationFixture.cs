namespace Nancy.Tests.Unit.Hosting
{
    using System.Linq;
    using Fakes;     
    using Xunit;
    using Nancy.Configuration;

    public class NancyApplicationFixture
    {
        [Fact]
        public void Should_return_null_for_an_unknown_view_extension()
        {
            NancyBootstrapper.BootstrapApplication().GetTemplateProcessor(".unknown").ShouldBeNull();
        }


        [Fact]
        public void Should_return_the_processor_for_a_given_extension()
        {
            NancyBootstrapper.BootstrapApplication().GetTemplateProcessor(".leto2").ShouldBeSameAs(FakeViewEngineRegistry.Executor);
        }

        [Fact]
        public void Should_be_case_intensitive_about_view_extensions()
        {
            NancyBootstrapper.BootstrapApplication().GetTemplateProcessor(".LetO2").ShouldBeSameAs(FakeViewEngineRegistry.Executor);
        }

        [Fact]
        public void Should_Return_All_Modules()
        {
            var modules = NancyBootstrapper.BootstrapApplication().ModuleMetas;
            modules.Count.ShouldEqual(4);
            modules["GET"].Count().ShouldEqual(2);
            modules["POST"].Count().ShouldEqual(2);
            modules["PUT"].Count().ShouldEqual(2);
            modules["DELETE"].Count().ShouldEqual(2);
        }
    }
}