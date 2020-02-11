using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class Group
    {
        [Key]
        public int Group_Id { get; set; }
        public string Group_Name { get; set; }
    }
}
