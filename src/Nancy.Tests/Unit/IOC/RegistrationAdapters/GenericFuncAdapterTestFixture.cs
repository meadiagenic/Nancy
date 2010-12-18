namespace Nancy.Tests.Unit.IOC.RegistrationAdapters
{
    using System;
using Xunit;
    using Nancy.IOC;

    public class GenericFuncAdapterTestFixture
    {
        [Fact]
        public void Can_Resolve_Func_Of_Registered_Service()
        {
            using (var c = new NancyContainer())
            {
                c.Register(typeof(IFoo), container => new Foo());

                var fooFunc = c.Resolve(typeof(Func<IFoo>));

                fooFunc.ShouldNotBeNull();
                fooFunc.ShouldBeOfType(typeof(Func<IFoo>));

                var fooResult = ((Func<IFoo>)fooFunc)();
                fooResult.ShouldNotBeNull();
                fooResult.ShouldBeOfType<IFoo>();
                fooResult.ShouldBeOfType<Foo>();
            }
        }

        [Fact]
        public void Can_Resolve_Func_Of_Named_Service()
        {
            using (var c = new NancyContainer())
            {
                c.Register("Foo2", typeof(IFoo), c2 => new Foo2());
                c.Register(typeof(IFoo), c2 => new Foo());

                var fooFunc = c.Resolve("Foo2", typeof(Func<IFoo>));

                fooFunc.ShouldNotBeNull();
                fooFunc.ShouldBeOfType<Func<IFoo>>();

                var fooResult = ((Func<IFoo>)fooFunc)();

                fooResult.ShouldNotBeNull();
                fooResult.ShouldBeOfType<IFoo>();
                fooResult.ShouldBeOfType<Foo2>();
            }
        }

    }
}
