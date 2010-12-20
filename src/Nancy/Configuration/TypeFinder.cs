namespace Nancy.Configuration
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;

    public class TypeFinder
    {
        public static Assembly FindCallingAssembly()
        {
            var trace = new StackTrace(false);

            var thisAssembly = Assembly.GetExecutingAssembly();
            var nancy = typeof(TypeFinder).Assembly;

            Assembly callingAssembly = null;
            for (var i = 0; i < trace.FrameCount; i++)
            {
                var frame = trace.GetFrame(i);
                var assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly != thisAssembly && assembly != nancy)
                {
                    callingAssembly = assembly;
                    break;
                }
            }
            return callingAssembly;
        }

        private IList<Assembly> _assemblies = new List<Assembly>();

        public TypeFinder() : this(FindCallingAssembly()) {

        }

        public TypeFinder(Assembly defaultAssembly)
        {
            _assemblies.Add(defaultAssembly);
        }

        private bool _scanned;
        private List<Type> _types = new List<Type>();
        public IEnumerable<Type> Types
        {
            get
            {
                if (!_scanned)
                {
                    ScanAssemblies();
                }
                return _types;
            }
        }

        private static object scanLocker = new object();
        private void ScanAssemblies()
        {
            lock (scanLocker)
            {
                _scanned = true;
                _types.Clear();
                _types.AddRange(Assemblies.SelectMany(a => a.GetExportedTypes()));
            }
        }

        public IEnumerable<Type> TypesMatching(Func<Type, bool> filter)
        {
            return Types.Where(filter).Distinct();
        }

        public void AddAssembly(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }

        public void AddAssembly(string assemblyName)
        {
            AddAssembly(Assembly.Load(assemblyName));
        }

        public IEnumerable<Assembly> Assemblies
        {
            get
            {
                foreach (var a in _assemblies.Distinct())
                {
                    yield return a;
                }
            }
        }

    }
}
