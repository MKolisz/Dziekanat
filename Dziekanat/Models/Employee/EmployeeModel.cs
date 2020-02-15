using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Models.Employee
{
    public class EmployeeModel
    {
        public int Employee_Id { get; set; }
        public int Department_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
    }
}
