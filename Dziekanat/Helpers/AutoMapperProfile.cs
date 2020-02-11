using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Models.Employee;
using Dziekanat.Models.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<StudentRegisterModel, Student>();

            CreateMap<EmployeeRegisterModel, Employee>();
        }
    }
}
