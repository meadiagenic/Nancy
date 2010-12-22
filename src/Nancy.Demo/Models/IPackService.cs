using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nancy.Demo.Models
{
    public interface IPackService
    {
        RatPack GetPackMember(string name);
    }
}