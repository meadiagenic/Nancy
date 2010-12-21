using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Configuration;
using FakeItEasy;
using Xunit;

namespace Nancy.Tests.Unit.Configuration
{
    public class AssemblyLoaderTestFixture
    {
        [Fact]
        public void InBinFolder_Returns_All_Dlls()
        {
            var tf = new TypeFinder();
            var loader = new AssemblyLoader(tf);
            loader.InBinFolder();

            tf.ShouldNotBeNull();
            tf.Assemblies.Count().ShouldEqual(6);
        }
    }
}
