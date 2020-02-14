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
        private IEmployeeService _employeeService;
        private IMapper _mapper;

        public EmployeeController(EmployeeContext employeeContext, IEmployeeService employeeService, IMapper mapper)
        {
            _employeeContext = employeeContext;
            _employeeService = employeeService;
            _mapper = mapper;
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