using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.IOC.ReuseScopes;

namespace Nancy.IOC
{
    public static class ReuseScope
    {
        static ReuseScope()
        {
            Singleton = new SingletonScope();
            AlwaysNew = new AlwaysNewScope();
        }

        public static IReuseScope Singleton { get; private set; }
        public static IReuseScope AlwaysNew { get; private set; }
    }
}
