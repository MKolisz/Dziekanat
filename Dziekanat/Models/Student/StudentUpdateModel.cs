using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Models.Student
{
    public class StudentUpdateModel
    {
        public int Department_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Field_Of_Study { get; set; }
        public int Semester { get; set; }
    }
}
