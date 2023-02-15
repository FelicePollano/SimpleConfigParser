﻿using System;
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
        readonly Dictionary<string, Section> sections = new Dictionary<string, Section>();

        public Dictionary<string, Section> Sections => sections;

        public void  Add(string s)
        {
            current = this;
            var items = Parser.config.Parse(s);
        }
    }
}
