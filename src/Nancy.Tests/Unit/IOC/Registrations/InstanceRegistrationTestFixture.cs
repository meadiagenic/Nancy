using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Nancy.IOC.Registrations;
using Nancy.IOC;

namespace Nancy.Tests.Unit.IOC.Registrations
{
    public class InstanceRegistrationTestFixture
    {
        [Fact]
        public void Null_Instance_Returns_Null()
        {
            var reg = new InstanceRegistration(typeof(object), null, null);

            var instance = reg.Instance;
            instance.ShouldBeNull();
        }

        [Fact]
        public void Object_Disposed_After_Creation_Returns_Null()
        {
            var disposable = new DisposableClass();
            var reg = new InstanceRegistration(typeof(DisposableClass), disposable, null);
            
            disposable = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var instance = reg.Instance;
            instance.ShouldBeNull();
        }

        [Fact]
        public void Instance_On_Creation_Is_Instance_Returned()
        {
            var disposable = new DisposableClass();
            var reg = new InstanceRegistration(typeof(DisposableClass), disposable, null);

            var instance1 = reg.CreateInstance(null);

            var instance2 = reg.Instance;

            var instance3 = reg.GetInstance(null);

            disposable.ShouldBeSameAs(instance1);
            disposable.ShouldBeSameAs(instance2);
            disposable.ShouldBeSameAs(instance3);
        }

        [Fact]
        public void ResuseScope_Has_No_Impact_On_Instance()
        {
            var obj = new object();
            var reg = new InstanceRegistration(typeof(object), obj, null).WithinScope(ReuseScope.AlwaysNew);

            var instance = reg.GetInstance(null);

            reg.WithinScope(ReuseScope.Singleton);

            var instance2 = reg.GetInstance(null);

            instance.ShouldNotBeNull();
            instance.ShouldBeSameAs(instance2);
            instance.ShouldBeSameAs(obj);
        }
    }

    public class DisposableClass : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}
