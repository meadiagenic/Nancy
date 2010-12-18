namespace Nancy.Tests.Unit.IOC.RegistrationAdapters
{
    using System;
using Xunit;
    using Nancy.IOC;
    

    public class ConcreteAdapterTestFixture
    {
        [Fact]
        public void Can_Resolve_Concrete_Type_That_Was_Not_Registered()
        {
            using (var c = new NancyContainer())
            {
                var foo = c.Resolve(typeof(Foo));

                foo.ShouldNotBeNull();
                foo.ShouldBeOfType<Foo>();

            }
        }

        [Fact]
        public void Cannot_Resolve_Interface_That_Was_Not_Registered()
        {
            using (var c = new NancyContainer())
            {
                var foo = c.Resolve(typeof(IFoo));

                foo.ShouldBeNull();
            }
        }
    }
}
