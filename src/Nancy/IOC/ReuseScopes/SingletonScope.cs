using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.IOC.ReuseScopes
{
    public class SingletonScope : IReuseScope
    {
        public object GetInstance(NancyContainer container, IInstanceCreator creator)
        {
            if (creator.Instance == null)
            {
                creator.Instance = creator.CreateInstance(container);
            }
            return creator.Instance;
        }
    }
}
