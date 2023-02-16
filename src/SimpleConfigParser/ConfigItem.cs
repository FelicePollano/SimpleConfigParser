using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConfigParser
{
    public abstract class ConfigItem
    {
        public abstract void Apply(Configuration c);
    }
}
