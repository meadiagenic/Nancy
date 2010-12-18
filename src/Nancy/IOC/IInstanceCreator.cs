using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.IOC
{
    public interface IInstanceCreator
    {
        object CreateInstance(NancyContainer container);
        object Instance { get; set; }
    }
}
