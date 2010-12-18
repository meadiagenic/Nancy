using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.IOC
{
    public interface IReuseScope
    {
        object GetInstance(NancyContainer container, IInstanceCreator creator);
    }
}
