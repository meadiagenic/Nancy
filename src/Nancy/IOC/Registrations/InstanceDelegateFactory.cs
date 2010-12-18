namespace Nancy.IOC.Registrations
{
    using System;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    public static class InstanceDelegateFactory
    {
        public static Func<INancyContainer, object> CreateFactoryDelegate(this Type implementationType, NancyContainer container)
        {
            ConstructorInfo ctor = GetConstructor(implementationType, container);
            if (ctor == null) throw new InvalidOperationException("No constructor with resolvable parameters was found.");

            ParameterInfo[] parameters = ctor.GetParameters();

            ParameterExpression nancyContainer = Expression.Parameter(typeof(INancyContainer), "container");
            List<Expression> arguments = new List<Expression>();
            foreach (var parameter in parameters)
            {
                var p = Expression.Call(nancyContainer, "Resolve", Type.EmptyTypes, Expression.Constant(string.Empty, typeof(string)), Expression.Constant(parameter.ParameterType, typeof(Type)));
                var p2 = Expression.Convert(p, parameter.ParameterType);
                arguments.Add(p2);
            }

            NewExpression exp = Expression.New(ctor, arguments);

            return Expression.Lambda<Func<INancyContainer, object>>(
                    exp,
                    new ParameterExpression[] { nancyContainer }
                ).Compile();
        }

        private static ConstructorInfo GetConstructor(Type implementationType, INancyContainer container)
        {
            var constructors = implementationType.GetConstructors().OrderBy(c => c.GetParameters().Length).Reverse();
            foreach (var ctor in constructors)
            {
                bool canResolveParameters = ctor.GetParameters().All(p => container.CanResolve(p.ParameterType));
                if (canResolveParameters) return ctor;
            }
            return null;
        }
    }
}
