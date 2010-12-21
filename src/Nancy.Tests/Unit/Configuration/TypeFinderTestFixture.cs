namespace Nancy.Tests.Unit.Configuration
{
    using System;
    using Xunit;
    using System.Linq;
    using Nancy.Configuration;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web;

    public class TypeFinderTestFixture
    {
        [Fact]
        public void Default_Finder_Returns_One_Assembly()
        {
            var tf = new TypeFinder();
            tf.Assemblies.Count().ShouldEqual(1);
        }

        [Fact]
        public void Default_Finder_Returns_More_Than_One_Type()
        {
            var tf = new TypeFinder();

            tf.Types.Count();
        }

        [Fact]
        public void Can_Find_Types_Implementing_Interface()
        {
            var tf = new TypeFinder();
            tf.AddAssembly(GetType().Assembly);
            var iFooTypes = tf.TypesImplementing<ITypeFoo>();

            iFooTypes.ShouldNotBeNull();
            iFooTypes.Count().ShouldEqual(2);

        }

        [Fact]
        public void Can_Find_Types_Implementing_Base_Class()
        {
            var tf = new TypeFinder();
            tf.AddAssembly(GetType().Assembly);
            var fooType = tf.TypesImplementing<TypeFoo>();

            fooType.ShouldNotBeNull();
            fooType.Count().ShouldEqual(1);
        }

        
    }
}
