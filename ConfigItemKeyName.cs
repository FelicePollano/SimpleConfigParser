using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitConfigParser
{
    public  class ConfigItemKeyName:ConfigItem
    {
        public ConfigItemKeyName(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
