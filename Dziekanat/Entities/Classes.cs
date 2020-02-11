using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class Classes
    {
        public int Classes_Id { get; set; }
        public int Group_Id { get; set; }
        public int Subject_Id { get; set; }
        public int Employee_Id { get; set; }
        public DateTime Time { get; set; }
    }
}
