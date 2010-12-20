namespace Nancy.Configuration
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class TypeFinderExtensions
    {
        public static IEnumerable<Type> TypesImplementing<T>(this TypeFinder typeFinder)
        {
            Type typeofT = typeof(T);
            var filter = (typeofT.IsInterface) ? (Func<Type, bool>)( t => t.GetInterfaces().Contains(typeof(T)))
                : (Func<Type, bool>)(t => t.IsSubclassOf(typeofT));
            return typeFinder.TypesMatching(filter);
        }
    }
}
