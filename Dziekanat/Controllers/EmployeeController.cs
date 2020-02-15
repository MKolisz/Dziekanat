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
    [Authorize(Roles = "Admin,Lecturer,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _employeeContext;
        private readonly DepartmentContext _departmentContext;
        private IEmployeeService _employeeService;
        private IMapper _mapper;

        public EmployeeController(EmployeeContext employeeContext,DepartmentContext departmentContext, IEmployeeService employeeService, IMapper mapper)
        {
            _employeeContext = employeeContext;
            _employeeService = employeeService;
            _departmentContext = departmentContext;
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

        [Authorize(Roles ="Admin")]
        [HttpPut("{employeeId}")]
        public IActionResult Update(int employeeId, [FromBody]EmployeeUpdateModel model)
        {
            var employee = _mapper.Map<Employee>(model);

            try
            {
                _employeeService.Update(employee, employeeId);
                return Ok();
            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles ="Admin,Lecturer")]
        [HttpGet("myInfo")]
        public IActionResult GetMyInfo()
        {
            try
            {
                int myId = int.Parse(User.Identity.Name);
                var employee = _employeeService.GetById(myId);
                if (employee == null)
                    throw new AppException("Employee does not exist");
                return Ok(new
                {
                    Id = employee.Employee_Id,
                    Department = _departmentContext.Department.Find(employee.Department_Id).Department_Name,
                    First_Name = employee.First_Name,
                    Last_Name = employee.Last_Name,
                    Email = employee.Email
                });
            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Lecturer,Student")]
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployee(int employeeId)
        {
            try
            {
                var employee = _employeeService.GetById(employeeId);
                if (employee == null)
                    throw new AppException("Employee does not exist");
                return Ok(new
                {
                    Id = employee.Employee_Id,
                    Department = _departmentContext.Department.Find(employee.Department_Id).Department_Name,
                    First_Name = employee.First_Name,
                    Last_Name = employee.Last_Name,
                    Email = employee.Email
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                var employees = _employeeService.GetAll();
                var model = _mapper.Map<IList<EmployeeModel>>(employees);
                return Ok(model);
            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{employeeId}")]
        public IActionResult Delete(int employeeId)
        {
            try
            {
                // delete user
                _employeeService.Delete(employeeId);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}