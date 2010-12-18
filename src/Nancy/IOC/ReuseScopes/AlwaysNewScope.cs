using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.IOC.ReuseScopes
{
    public class AlwaysNewScope : IReuseScope
    {
        public object GetInstance(NancyContainer container, IInstanceCreator creator)
        {
            return creator.CreateInstance(container);
        }
    }
}
