using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace GitConfigParser
{
    public class Configuration:Section
    {
        Section current;

        public Section Current { get => current; set => current = value; }

        public void  Add(Stream s)
        {
            current = this;
            using (var sr = new StreamReader(s))
            {
                var items = Parser.config.Parse(sr.ReadToEnd());
                foreach (var item in items)
                {
                    item.Apply(this);
                }
            }
        }
    }
}
