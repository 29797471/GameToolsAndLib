using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CqCore
{
    public interface IObjectAttribute
    {
        void SetTarget(object parent);
    }
}
