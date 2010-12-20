namespace Nancy.Extensions
{
    using System;
    using System.Linq;  
    using System.Collections.Generic;

    public static class TypeExtensions
    {
        public static IEnumerable<Type> ConcreteClasses(this IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass);
        }

        public static IEnumerable<Type> CanCreateInstance(this IEnumerable<Type> types)
        {
            return types.Where(t => t.GetConstructor(Type.EmptyTypes) != null);
        }
    }
}
