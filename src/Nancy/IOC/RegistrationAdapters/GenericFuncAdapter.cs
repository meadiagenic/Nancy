using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Nancy.IOC.Registrations;

namespace Nancy.IOC.RegistrationAdapters
{
    public class GenericFuncAdapter : IRegistrationAdapter
    {
        public IEnumerable<IRegistration> GetRegistrationsFor(NancyContainer container, Type serviceType)
        {
            if (serviceType != null && serviceType.IsGenericTypeDefinedBy(typeof(Func<>)))
            {
                var baseServiceType = serviceType.FunctionReturnType();
                var baseServiceRegistrations = container.GetRegistrations(baseServiceType, null);

                var newRegistrations = new List<IRegistration>();
                foreach (var registration in baseServiceRegistrations)
                {
                    var factory = FuncGenerator.CreateDelegate(serviceType, container, registration.Name);

                    newRegistrations.Add(new FactoryRegistration(serviceType, c => factory, registration.Name));
                }
                return newRegistrations.AsEnumerable();
            }

            return Enumerable.Empty<IRegistration>();
        }

        public bool CanCreateRegistrationsFor(Type serviceType)
        {
            return serviceType.IsGenericTypeDefinedBy(typeof(Func<>));
        }
    }

    public static class FuncGenerator
    {
        public static object CreateDelegate(Type delegateType, NancyContainer nancyContainer, string serviceName)
        {
            var returnType = delegateType.FunctionReturnType();
            var exps = new Expression[] {
                Expression.Constant(serviceName),
                Expression.Constant(returnType)
            };

            var container = Expression.Constant(nancyContainer);
            var exp = Expression.Call(container, nancyContainer.GetMethod(nc => nc.Resolve(string.Empty, default(Type))), exps);

            var expConvert = Expression.Convert(exp, returnType);

            var g = Expression.Lambda(delegateType, expConvert).Compile();

            return (object)g;
        }
    }

}
