﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitConfigParser
{
    public class Section:Dictionary<string,string>
    {
        readonly Dictionary<string, Section> sections = new Dictionary<string, Section>();
        public Dictionary<string, Section> Sections => sections;
    }
}
