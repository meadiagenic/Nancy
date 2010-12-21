namespace Nancy
{
    using System;
    using System.IO;
    using Nancy.IOC;

    public interface INancyApplication
    {
        INancyContainer Container { get; }
        Func<string, object, Action<Stream>> GetTemplateProcessor(string extension);
        Func<string, object, Action<Stream>> DefaultProcessor { get; }
        INancyEngine GetEngine();
    }
}