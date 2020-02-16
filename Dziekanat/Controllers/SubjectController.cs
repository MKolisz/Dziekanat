using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Helpers;
using Dziekanat.Models.Subject;
using Dziekanat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dziekanat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectContext _subjectContext;
        private ISubjectService _subjectService;
        private IMapper _mapper;

        public SubjectController(SubjectContext subjectContext, ISubjectService subjectService, IMapper mapper)
        {
            _subjectContext = subjectContext;
            _subjectService = subjectService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addSubject")]
        public IActionResult Create([FromBody]SubjectCreateModel model)
        {
            var subject = _mapper.Map<Subject>(model);
            try
            {
                _subjectService.Create(subject);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{subjectId}")]
        public IActionResult Update(int subjectId, [FromBody]SubjectCreateModel model)
        {
            var subject = _mapper.Map<Subject>(model);

            try
            {
                _subjectService.Update(subject, subjectId);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Lecturer,Student")]
        [HttpGet("{subjectId}")]
        public IActionResult GetSubject(int subjectId)
        {
            try
            {
                var subject = _subjectService.GetById(subjectId);
                if (subject == null)
                    throw new AppException("Subject does not exist");
                return Ok(new
                {
                    Id = subject.Subject_Id,
                    Name=subject.Subject_Name
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Lecturer,Student")]
        [HttpGet]
        public IActionResult GetSubjects()
        {
            var subjects = _subjectService.GetAll();
            var model = _mapper.Map<IList<SubjectModel>>(subjects);
            return Ok(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{subjectId}")]
        public IActionResult Delete(int subjectId)
        {
            try
            {
                _subjectService.Delete(subjectId);
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