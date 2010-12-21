namespace Nancy
{
    using System;    
    using System.IO;
    using Nancy.IOC;
    using System.Collections.Generic;

    public interface INancyApplication
    {
        Func<string, object, Action<Stream>> GetTemplateProcessor(string extension);
        Func<string, object, Action<Stream>> DefaultProcessor { get; }
        IDictionary<string, Func<string, object, Action<Stream>>> TemplateProcessors { get; }
        IDictionary<string, IEnumerable<ModuleMeta>> ModuleMetas { get; }
        INancyContainer Container { get; }
        INancyEngine GetEngine();
    }
}