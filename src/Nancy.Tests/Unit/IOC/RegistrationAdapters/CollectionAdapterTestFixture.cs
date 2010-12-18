namespace Nancy.Tests.Unit.IOC.RegistrationAdapters
{
    using System;
    using Xunit;
    using Nancy.IOC;
    using System.Linq;
    using System.Collections.Generic;

    public class CollectionAdapterTestFixture
    {
        [Fact]
        public void Can_Resolve_IEnumerable_Of_Services()
        {
            using (var c = new NancyContainer())
            {
                c.Register(typeof(IFoo), typeof(Foo));
                c.Register(typeof(IFoo), typeof(Foo2));

                var fooEnum = c.Resolve(typeof(IEnumerable<IFoo>));

                fooEnum.ShouldNotBeNull();
                fooEnum.ShouldBeOfType(typeof(IEnumerable<IFoo>));

                var foo = (IEnumerable<IFoo>)fooEnum;

                foo.Count().ShouldEqual(2);
            }
        }

        [Fact]
        public void Can_Resolve_Array_Of_Services()
        {
            using (var c = new NancyContainer())
            {
                c.Register(typeof(IFoo), typeof(Foo));
                c.Register(typeof(IFoo), typeof(Foo2));

                var fooArray = c.Resolve(typeof(IFoo[]));

                fooArray.ShouldNotBeNull();
                fooArray.ShouldBeOfType(typeof(IFoo[]));

                var foo = (IFoo[])fooArray;

                foo.Length.ShouldEqual(2);
            }
        }
    }
}
