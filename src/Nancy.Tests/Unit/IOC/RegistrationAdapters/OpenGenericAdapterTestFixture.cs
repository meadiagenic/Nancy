namespace Nancy.Tests.Unit.IOC.RegistrationAdapters
{
    using System;
    using Xunit;
    using Nancy.IOC;

    public class OpenGenericAdapterTestFixture
    {
        [Fact]
        public void Can_Resolve_Closed_Generic_If_Open_Generic_Registered()
        {
            using (var c = new NancyContainer())
            {
                c.Register(typeof(IGeneric<>), typeof(Generic<>));

                var gen = c.Resolve(typeof(IGeneric<int>));

                gen.ShouldNotBeNull();
                gen.ShouldBeOfType<IGeneric<int>>();
                gen.ShouldBeOfType<Generic<int>>();
            }
        }

        [Fact]
        public void Can_Resolve_Close_Generic_When_Open_Generic_Has_Dependencies()
        {
            using (var c = new NancyContainer())
            {
                c.Register(typeof(IGeneric<>), typeof(Generic2<>));
                c.Register(typeof(IFoo), typeof(Foo));

                var gen = c.Resolve(typeof(IGeneric<IFoo>));

                gen.ShouldNotBeNull();
                gen.ShouldBeOfType<Generic2<IFoo>>();

                var strongGen = gen as Generic2<IFoo>;
                strongGen.ShouldNotBeNull();

                strongGen.Foo.ShouldNotBeNull();
                strongGen.Foo.ShouldBeOfType<Foo>();
            }
        }

        interface IGeneric<T>
        {

        }

        class Generic<T> : IGeneric<T>
        {

        }

        class Generic2<T> : IGeneric<T>
        {
            public Generic2(T foo)
            {
                Foo = foo;
            }

            public T Foo { get; set; }
        }
    }
}
