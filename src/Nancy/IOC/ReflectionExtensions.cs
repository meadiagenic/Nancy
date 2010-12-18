using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Nancy.IOC
{
    public static class ReflectionExtensions
    {
        public static bool IsDelegate(this Type type)
        {
            return type.IsSubclassOf(typeof(Delegate));
        }

        public static Type FunctionReturnType(this Type type)
        {
            var invoke = type.GetMethod("Invoke");
            return invoke.ReturnType;
        }

        public static bool IsGenericTypeDefinedBy(this Type type, Type genericType)
        {
            if (type == null) return false;
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == genericType));
        }

        public static MethodInfo GetMethod<T>(this T type, Expression<Action<T>> methodExpression)
        {
            var callExpression = methodExpression.Body as MethodCallExpression;
            return callExpression.Method;
        }
    }
}
