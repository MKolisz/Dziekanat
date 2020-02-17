using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Helpers;
using Dziekanat.Models.Grade;
using Dziekanat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dziekanat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly GradeContext _gradeContext;
        private IGradeService _gradeService;
        private IMapper _mapper;

        public GradeController(GradeContext gradeContext, IGradeService gradeService, IMapper mapper)
        {
            _gradeContext = gradeContext;
            _gradeService = gradeService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Lecturer")]
        [HttpPost("{studentId}")]
        public IActionResult Create([FromBody]GradeCreateModel model, int studentId)
        {
            var grade = _mapper.Map<Grade>(model);
            try
            {
                _gradeService.Create(grade, studentId);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Lecturer")]
        [HttpPut("{gradeId}")]
        public IActionResult Update(int gradeId, [FromBody]GradeUpdateModel model)
        {
            var grade = _mapper.Map<Grade>(model);

            try
            {
                _gradeService.Update(grade, gradeId);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Student")]
        [HttpGet("myGrades/{subjectId}")]
        public IActionResult GetMyGrades(int subjectId)
        {
            try
            {
                int myId = int.Parse(User.Identity.Name);
                var grades = _gradeService.GetGrades(myId, subjectId);

                var model = _mapper.Map<IList<GradeModel>>(grades);
                return Ok(model);

            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet("{studentId}/{subjectId}")]
        public IActionResult GetGrades(int studentId, int subjectId)
        {
            try
            {
                var grades = _gradeService.GetGrades(studentId, subjectId);

                var model = _mapper.Map<IList<GradeModel>>(grades);
                return Ok(model);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Lecturer")]
        [HttpDelete("{gradeId}")]
        public IActionResult Delete(int gradeId)
        {
            try
            {
                _gradeService.Delete(gradeId);
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