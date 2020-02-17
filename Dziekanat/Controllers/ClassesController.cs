using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Helpers;
using Dziekanat.Models.Classes;
using Dziekanat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dziekanat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ClassesContext _classesContext;
        private IClassesService _classesService;
        private IMapper _mapper;

        public ClassesController(ClassesContext classesContext, IClassesService classesService, IMapper mapper)
        {
            _classesContext = classesContext;
            _classesService = classesService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addClasses")]
        public IActionResult Create([FromBody]ClassesJsonModel model)
        {
             string[] splt = model.Time.Split(".");
             DateTime time = new DateTime(Int32.Parse(splt[2]), Int32.Parse(splt[1]), Int32.Parse(splt[0]),
                 Int32.Parse(splt[3]), Int32.Parse(splt[4]), Int32.Parse(splt[5]));

               ClassesCreateModel validModel = new ClassesCreateModel()
                {
                   Employee_Id = model.Employee_Id,
                   Group_Id = model.Group_Id,
                   Subject_Id = model.Subject_Id,
                   Time = time
                };

            var classes = _mapper.Map<Classes>(validModel);
            try
            {
                _classesService.Create(classes);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{classesId}")]
        public IActionResult Update(int classesId, [FromBody]ClassesJsonModel model)
        {
            string[] splt = model.Time.Split('.');
            DateTime time = new DateTime(Int32.Parse(splt[2]), Int32.Parse(splt[1]), Int32.Parse(splt[0]),
                Int32.Parse(splt[3]), Int32.Parse(splt[4]), Int32.Parse(splt[5]));

            ClassesCreateModel validModel = new ClassesCreateModel()
            {
                Employee_Id = model.Employee_Id,
                Group_Id = model.Group_Id,
                Subject_Id = model.Subject_Id,
                Time = time
            };

            var classes = _mapper.Map<Classes>(validModel);
            try
            {
                _classesService.Update(classes, classesId);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin,Lecturer,Student")]
        [HttpGet("{groupId}")]
        public IActionResult GetClassesForWeek(int groupId)
        {
            try
            {
                var classes = _classesService.GetAll(groupId);
                var model = _mapper.Map<IList<ClassesModel>>(classes);
                return Ok(model);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{classesId}")]
        public IActionResult Delete(int classesId)
        {
            try
            {
                _classesService.Delete(classesId);
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