using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Entities
{
    public class File
    {
        [Key]
        public int File_Id { get; set; }
        public int Employee_Id { get; set; }
        public string File_Name { get; set; }
        public string Storage_Name { get; set; }
    }
}
