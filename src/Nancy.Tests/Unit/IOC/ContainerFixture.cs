namespace Nancy.Tests.Unit.IOC
{
    using System;
    using Xunit;
    using Nancy.IOC;
    using System.Linq;
    using System.Collections.Generic;
    using Nancy.IOC.ReuseScopes;

    public class ContainerFixture
    {
        

        [Fact]
        public void Can_Create_Container()
        {
            using (var container = new NancyContainer())
            {
                container.ShouldNotBeNull();
            }
        }

        [Fact]
        public void Can_Resolve_Nameless_NonGeneric_Factory()
        {
            using (var container = new NancyContainer())
            {
                container.Register(typeof(IFoo), c => new Foo());

                var foo = container.Resolve(null, typeof(IFoo));

                foo.ShouldNotBeNull();
                foo.ShouldBeOfType<IFoo>();
                foo.ShouldBeOfType<Foo>();
            }
        }

        [Fact]
        public void Can_Resolve_Named_NonGeneric_Factory()
        {
            using (var container = new NancyContainer())
            {
                container.Register("foo", typeof(IFoo), c => new Foo());

                var foo = container.Resolve("foo", typeof(IFoo));

                foo.ShouldNotBeNull();
                foo.ShouldBeOfType<IFoo>();
                foo.ShouldBeOfType<Foo>();
            }
        }

        [Fact]
        public void Last_Registered_Instance_Is_Resolved()
        {
            using (var container = new NancyContainer())
            {
                container.Register(typeof(IFoo), c => new Foo());
                container.Register(typeof(IFoo), c => new Foo2());

                var foo = container.Resolve(typeof(IFoo));
                foo.ShouldNotBeNull();
                foo.ShouldBeOfType<IFoo>();
                foo.ShouldBeOfType<Foo2>();
            }
        }

        [Fact]
        public void Resolve_Without_Name_Returns_Named_Instance()
        {
            using (var container = new NancyContainer())
            {
                container.Register("foo1", typeof(IFoo), c => new Foo());

                var foo = container.Resolve(typeof(IFoo));

                foo.ShouldNotBeNull();
                foo.ShouldBeOfType<IFoo>();
                foo.ShouldBeOfType<Foo>();
            }
        }

        [Fact]
        public void Can_Register_And_Resolve_Multiple_Types()
        {
            using (var container = new NancyContainer())
            {
                container.Register(typeof(IBar), c => new Bar());
                container.Register(typeof(IFoo), c => new Foo());

                var foo = container.Resolve(null, typeof(IFoo));
                var bar = container.Resolve(null, typeof(IBar));
            }
        }

        [Fact]
        public void Different_Named_Registrations_Resolve_To_Different_Types()
        {
            using (var container = new NancyContainer())
            {
                container.Register("foo1", typeof(IFoo), c => new Foo());
                container.Register("foo2", typeof(IFoo), c => new Foo2());

                var foo1 = container.Resolve("foo1", typeof(IFoo));
                var foo2 = container.Resolve("foo2", typeof(IFoo));

                foo1.ShouldNotBeNull();
                foo1.ShouldBeOfType<Foo>();

                foo2.ShouldNotBeNull();
                foo2.ShouldBeOfType<Foo2>();
            }
        }

        [Fact]
        public void Can_Resolve_Unnamed_Type()
        {
            using (var c = new NancyContainer())
            {
                c.Register(typeof(IFoo), typeof(Foo));

                var foo = c.Resolve(typeof(IFoo));

                foo.ShouldNotBeNull();
                foo.ShouldBeOfType<IFoo>();
                foo.ShouldBeOfType<Foo>();
            }
        }

        [Fact]
        public void Can_Resolve_Unnamed_Type_With_Dependencies()
        {
            using (var c = new NancyContainer())
            {
                c.Register(typeof(IFoo), container => new Foo());
                c.Register(typeof(IBar), typeof(FooBar));

                var fooBar = c.Resolve(typeof(IBar)) as FooBar;

                fooBar.ShouldNotBeNull();
                fooBar.Foo.ShouldNotBeNull();
            }
        }

        [Fact]
        public void Can_Resolve_Unamed_Instance()
        {
            using (var c = new NancyContainer())
            {
                var originalFoo = new Foo();
                c.Register(typeof(IFoo), originalFoo);

                var foo = c.Resolve(typeof(IFoo));
                foo.ShouldNotBeNull();
                foo.ShouldBeSameAs(originalFoo);
            }
        }

        [Fact]
        public void Resolve_With_AlwaysNew_Scope_Returns_Different_Instance()
        {
            using (var c = new NancyContainer())
            {
                c.Register(string.Empty, typeof(IFoo), container => new Foo()).WithinScope(new AlwaysNewScope());

                var foo1 = c.Resolve(typeof(IFoo));
                var foo2 = c.Resolve(typeof(IFoo));

                foo1.ShouldNotBeNull();
                foo1.ShouldBeOfType<Foo>();
                foo2.ShouldNotBeNull();
                foo2.ShouldBeOfType<Foo>();

                foo1.ShouldNotBeSameAs(foo2);
            }
        }

        [Fact]
        public void Resolve_With_Singleton_Scope_Returns_Same_Instance()
        {
            using (var c = new NancyContainer())
            {
                c.Register(string.Empty, typeof(IFoo), container => new Foo()).WithinScope(new SingletonScope());

                var foo1 = c.Resolve(typeof(IFoo));
                var foo2 = c.Resolve(typeof(IFoo));

                foo1.ShouldNotBeNull();
                foo1.ShouldBeOfType<Foo>();
                foo2.ShouldNotBeNull();
                foo2.ShouldBeOfType<Foo>();

                foo1.ShouldBeSameAs(foo2);
            }
        }
        
    }
}
