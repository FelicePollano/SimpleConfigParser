using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConfigParser
{
    public class ConfigItemSection:ConfigItem
    {
        public ConfigItemSection(string name,ConfigItemComment comment, string subsection)
        {
            Name = name;
            Comment = comment;
            Subsection = subsection;
        }

        public string Name { get; }
        public ConfigItemComment Comment { get; }
        public string Subsection { get; }

        public override void Apply(Configuration c)
        {
            if(!c.Sections.ContainsKey(Name)) 
            {
                c.Sections[Name] = new Section();
            }
            c.Current = c.Sections[Name];
            if (null != Subsection)
            {
                if (!c.Current.Sections.ContainsKey(Subsection))
                {
                    c.Sections[Name].Sections[Subsection] = new Section();
                }
                c.Current = c.Sections[Name].Sections[Subsection];
            }
        }
    }
}
