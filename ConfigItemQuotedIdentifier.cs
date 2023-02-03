using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitConfigParser
{
    public class ConfigItemQuotedIdentifier:ConfigItem
    {
        public ConfigItemQuotedIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
