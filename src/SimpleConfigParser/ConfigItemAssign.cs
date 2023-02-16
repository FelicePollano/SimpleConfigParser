using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConfigParser
{
    public class ConfigItemAssign:ConfigItem
    {
        
        public ConfigItemAssign(string lhs,string rhs,ConfigItemComment comment)
        {
            Lhs = lhs;
            Rhs = rhs;
            Comment = comment;
        }

        public string Lhs { get; }
        public string Rhs { get; }
        public ConfigItemComment Comment { get; }

        public override void Apply(Configuration c)
        {
            c.Current[Lhs] = Rhs==null?true.ToString(CultureInfo.InvariantCulture): Rhs;
        }
    }
}
