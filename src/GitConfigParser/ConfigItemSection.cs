﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitConfigParser
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
    }
}