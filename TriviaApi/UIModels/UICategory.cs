﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TriviaApi.UIModels
{
    public class UICategory
    {
        public string Id { get; set; }
        public  string Name { get; set; }
        public  IEnumerable<string> Questions { get; set; }
    }
}
