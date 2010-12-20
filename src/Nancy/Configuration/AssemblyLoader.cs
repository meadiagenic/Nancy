namespace Nancy.Configuration
{
    using System;
    using System.IO;
    using System.Reflection;

    public class AssemblyLoader
    {
        TypeFinder _typeFinder;

        public AssemblyLoader(TypeFinder finder)
        {
            _typeFinder = finder;
        }

        public AssemblyLoader InBinFolder()
        {
            var binPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath ?? AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            foreach (var file in Directory.GetFiles(binPath, "*.dll", SearchOption.AllDirectories))
            {
                _typeFinder.AddAssembly(Path.GetFileNameWithoutExtension(file));
            }
            return this;
        }
    }
}
