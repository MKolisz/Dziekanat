using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Helpers;
using Dziekanat.Models.Student;
using Dziekanat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dziekanat.Controllers
{
    [Authorize(Roles = "Admin,Lecturer,Student")]
    [Route("api/[controller]")]
    [ApiController] 
    public class StudentController : ControllerBase
    {

        private readonly StudentContext _studentContext;
        private IStudentService _studentService;
        private IMapper _mapper;

        private readonly DepartmentContext _departmentContext;

        public StudentController(StudentContext studentContext, IStudentService studentService, IMapper mapper, DepartmentContext departmentContext)
        {
            _studentContext = studentContext;
            _studentService = studentService;
            _mapper = mapper;
            _departmentContext = departmentContext;
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
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("{studentId}")]
        public IActionResult Update(int studentId, [FromBody]StudentUpdateModel model)
        {
            var student = _mapper.Map<Student>(model);

            try
            {
                _studentService.Update(student, studentId);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("uploadImage/{studentId}")]
        public IActionResult UploadImage(int studentId, IFormFile imgFile)
        {
            byte[] bytes;
            using (var reader = new StreamReader(imgFile.OpenReadStream()))
            {
                var x = reader.ReadToEnd();
                bytes = Encoding.ASCII.GetBytes(x);
            }
            try
            {
                _studentService.UploadImage(studentId, bytes);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize(Roles = "Student")]
        [HttpGet("myInfo")]
        public IActionResult GetMyInfo()
        {
            try
            {
                int myId = int.Parse(User.Identity.Name);
                var student = _studentService.GetById(myId);
                if(student==null)
                    throw new AppException("Student does not exist");
                return Ok(new
                {
                    Id = student.Student_Id,
                    Department = _departmentContext.Department.Find(student.Department_Id).Department_Name,
                    First_Name = student.First_Name,
                    Last_Name = student.Last_Name,
                    Field_Of_Study = student.Field_Of_Study,
                    Semester = student.Semester,
                    Email = student.Email
                });

            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Lecturer")]
        [HttpGet("{studentId}")]
        public IActionResult GetStudent(int studentId)
        {
            try
            {
                var student = _studentService.GetById(studentId);
                if (student == null)
                    throw new AppException("Student does not exist");
                return Ok(new
                {
                    Id = student.Student_Id,
                    Department = _departmentContext.Department.Find(student.Department_Id).Department_Name,
                    First_Name = student.First_Name,
                    Last_Name = student.Last_Name,
                    Field_Of_Study = student.Field_Of_Study,
                    Semester = student.Semester,
                    Email = student.Email
                });

            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles ="Admin,Lecturer")]
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _studentService.GetAll();
            var model = _mapper.Map<IList<StudentModel>>(students);
            return Ok(model);
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("{studentId}")]
        public IActionResult Delete(int studentId)
        {
            try
            {
                // delete user
                _studentService.Delete(studentId);
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