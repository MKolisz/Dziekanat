using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class Subject
    {
        [Key]
        public int Subject_Id { get; set; }
        public string Subject_Name { get; set; }
    }
}
