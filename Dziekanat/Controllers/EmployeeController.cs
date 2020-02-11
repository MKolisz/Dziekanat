using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Helpers;
using Dziekanat.Models.Employee;
using Dziekanat.Models.Student;
using Dziekanat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dziekanat.Controllers
{
    [Authorize(Roles = "Admin,Lecturer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _employeeContext;
        private readonly StudentContext _studentContext;
        private IEmployeeService _employeeService;
        private IStudentService _studentService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public EmployeeController(EmployeeContext employeeContext, StudentContext studentContext,
            IEmployeeService employeeService, IStudentService studentService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _employeeContext = employeeContext;
            _studentContext = studentContext;
            _employeeService = employeeService;
            _studentService = studentService;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("registerStudent")]
        public IActionResult RegisterStudent([FromBody]StudentRegisterModel model)
        {
            var student = _mapper.Map<Student>(model);
            try
            {
                _studentService.Create(student, model.Password);
                return Ok();
            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("registerEmployee")]
        public IActionResult RegisterEmployee([FromBody]EmployeeRegisterModel model)
        {
            var employee = _mapper.Map<Employee>(model);
            try
            {
                _employeeService.Create(employee, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}