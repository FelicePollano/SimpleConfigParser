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
        Dictionary<string, Section> sections = new Dictionary<string, Section>();
       
        public void  Add(Stream s)
        {
            current = this;
            using (var sr = new StreamReader(s))
            {
                var items = Parser.config.Parse(sr.ReadToEnd());
            }
        }
    }
}
