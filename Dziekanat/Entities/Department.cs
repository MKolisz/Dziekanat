using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class Department
    {
        [Key]
        public int Department_Id { get; set; }
        public string Department_Name { get; set; }
        public int Dean_Id { get; set; }
    }
}
