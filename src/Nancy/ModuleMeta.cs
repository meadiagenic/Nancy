namespace Nancy
{
    using System;
    using System.Collections.Generic;
    using Routing;

    public class ModuleMeta
    {
        
        public string TypeName { get; set; }
        public IEnumerable<RouteDescription> RouteDescriptions { get; set; }

        public ModuleMeta(Type type, IEnumerable<RouteDescription> routeDescriptions)
        {
            TypeName = type.FullName;
            RouteDescriptions = routeDescriptions;
        }
    }
}