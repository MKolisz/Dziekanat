using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Models.Employee;
using Dziekanat.Models.Grade;
using Dziekanat.Models.Student;
using Dziekanat.Models.Subject;
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
            CreateMap<Student, StudentModel>();
            CreateMap<StudentRegisterModel, Student>();
            CreateMap<StudentUpdateModel, Student>();

            CreateMap<Employee, EmployeeModel>();
            CreateMap<EmployeeRegisterModel, Employee>();
            CreateMap<EmployeeUpdateModel, Employee>();

            CreateMap<Subject, SubjectModel>();
            CreateMap<SubjectCreateModel, Subject>();

            CreateMap<Grade, GradeModel>();
            CreateMap<GradeCreateModel, Grade>();
            CreateMap<GradeUpdateModel, Grade>();
        }
    }
}
