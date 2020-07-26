using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TriviaApi.UIModels
{
    public class UICategory
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        public  IEnumerable<int> Questions { get; set; }
    }
}
