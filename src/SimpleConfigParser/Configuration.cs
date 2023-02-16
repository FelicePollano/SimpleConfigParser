using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace SimpleConfigParser
{
    public class Configuration:Section
    {
        Section current;

        readonly Dictionary<string, Section> sections = new Dictionary<string, Section>();

        public Dictionary<string, Section> Sections => sections;
        public Section Current { get => current; set => current = value; }
        public void  Add(string s)
        {
            current = this;
            var items = Parser.config.Parse(s);

            foreach (var item in items)
            {
                item.Apply(this);
            }
        }
    }
}
