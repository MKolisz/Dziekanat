using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Models.Student
{
    public class StudentModel
    {
        public int Student_Id { get; set; }
        public int Department_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Field_Of_Study { get; set; }
        public int Semester { get; set; }
        public string Email { get; set; }
    }
}
