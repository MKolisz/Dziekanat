using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class Grade
    {
        [Key]
        public int Grade_Id { get; set; }
        public double Grade_Value { get; set; }
        public int Subject_Id { get; set; }
        public int Student_Id { get; set; }
    }
}
