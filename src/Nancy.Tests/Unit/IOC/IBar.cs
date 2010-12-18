namespace Nancy.Tests.Unit.IOC
{
    using System;

    public interface IBar
    {
    }

    public class Bar : IBar
    {
    }

    public class FooBar : IBar
    {
        public FooBar(IFoo foo)
        {
            Foo = foo;
        }

        public IFoo Foo { get; set; }
    }
}
