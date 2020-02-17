using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dziekanat.Entities;
using Dziekanat.Helpers;
using Dziekanat.Models.Group;
using Dziekanat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dziekanat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly GroupContext _groupContext;
        private IGroupService _groupService;
        private IMapper _mapper;

        public GroupController(GroupContext groupContext, IGroupService groupService, IMapper mapper)
        {
            _groupContext = groupContext;
            _groupService = groupService;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("addGroup")]
        public IActionResult Create([FromBody]GroupCreateModel model)
        {
            var group = _mapper.Map<Group>(model);
            try
            {
                _groupService.Create(group);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{groupId}")]
        public IActionResult Update(int groupId, [FromBody]GroupCreateModel model)
        {
            var group = _mapper.Map<Group>(model);

            try
            {
                _groupService.Update(group, groupId);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //TODO
        /*
        [Authorize(Roles = "Student")]
        [HttpGet("myGroups")]
        public IActionResult GetMyGroups()
        {

        }*/

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetGroups()
        {
            var groups = _groupService.GetAll();
            var model = _mapper.Map<IList<GroupModel>>(groups);
            return Ok(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{groupId}")]
        public IActionResult Delete(int groupId)
        {
            try
            {
                _groupService.Delete(groupId);
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