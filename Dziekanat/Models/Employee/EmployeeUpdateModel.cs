using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Models.Employee
{
    public class EmployeeUpdateModel
    {
        public int Department_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
    }
}
