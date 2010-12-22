using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nancy.Demo.Models
{
    public class DefaultPackService :IPackService
    {
        public DefaultPackService()
        {
            var a = "";
        }

        public RatPack GetPackMember(string name)
        {
            return new RatPack() { FirstName = name ?? "Frank" };
        }
    }
}