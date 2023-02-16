using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConfigParser
{
    public class ConfigItemComment:ConfigItem
    {
        public ConfigItemComment(string prefix,string comment)
        {
            Prefix = prefix;
            Comment = comment;
        }

        public string Prefix { get; }
        public string Comment { get; }

        public override void Apply(Configuration c)
        {
            //swallow comments, till now
        }
    }
}
