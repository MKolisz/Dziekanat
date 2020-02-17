using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Models.Grade
{
    public class GradeModel
    {
        public int Grade_Id { get; set; }
        public double Grade_Value { get; set; }
        public int Subject_Id { get; set; }
    }
}
