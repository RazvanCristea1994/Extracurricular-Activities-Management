using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Activity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IActivityService _activityService;

        public ActivitiesController(IMapper mapper, UserManager<ApplicationUser> userManager, IActivityService activityService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _activityService = activityService;
        }

        /// <summary>
        /// Creates an activity
        /// <remarks>
        /// Sample request:
        /// Post /api/activities
        /// {
        ///     "name": "Rugby";
        ///     "description": "Only for 5th grade or upper";
        /// }
        /// </remarks>
        /// </summary>
        /// <param name="activityRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<Activity>> PostActivity(ActivityViewModel activityRequest)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!await _userManager.IsInRoleAsync(user, UserRole.ROLE_ADMIN))
            {
                return StatusCode(403);
            }

            var activityEntity = _mapper.Map<Activity>(activityRequest);
            
            var activityResponse = await _activityService.CreateActivity(activityEntity);

            if (activityResponse.ResponseError == null)
            {
                return CreatedAtAction("GetStory", new { id = activityEntity.Id }, activityRequest);
            }

            return StatusCode(500);
        }


        [HttpPost("{id}/activities")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> PostActivityForTeacher(int id, ActivityViewModel activity)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!await _userManager.IsInRoleAsync(user, UserRole.ROLE_ADMIN))
            {
                return StatusCode(403);
            }

            var commentResponse = await _activityService.AddActivityToTeacher(id, _mapper.Map<Activity>(activity));

            if (commentResponse.ResponseError == null)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("activities")]
		public async Task<ActionResult<List<ActivityViewModel>>> GetActivities()
		{
			var response = await _activityService.GetActivities();
			var activities = response.ResponseOk;

			if (activities == null)
			{
				return NotFound();
			}

			return activities;
		}

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityViewModel>> GetActivity(int id)
        {
            var response = await _activityService.GetActivity(id);
            var activity = response;

            if (activity == null)
            {
                return NotFound();
            }
            return _mapper.Map<ActivityViewModel>(activity);
        }

        [HttpPut("activities/{activityId}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> PutActivity(int activityId, ActivityViewModel activity)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!await _userManager.IsInRoleAsync(user, UserRole.ROLE_ADMIN))
            {
                return StatusCode(403);
            }

            if (activityId != activity.Id)
            {
                return BadRequest();
            }

            var commentResponse = await _activityService.UpdateActivity(_mapper.Map<Activity>(activity));

            if (commentResponse.ResponseError == null)
            {
                return NoContent();
            }

            return StatusCode(500);
        }


        [HttpDelete("{id}/activities/{activityId}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> RemoveActivityFromTeacher(int id, int activityId)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!await _userManager.IsInRoleAsync(user, UserRole.ROLE_ADMIN))
            {
                return StatusCode(403);
            }

            var serviceResponse = await _activityService.RemoveActivityFromTeacher(id, activityId);

            if (serviceResponse.ResponseError == null)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpDelete("Tags/{tagId}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> DeleteActivity(int activityId)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!await _userManager.IsInRoleAsync(user, UserRole.ROLE_ADMIN))
            {
                return StatusCode(403);
            }

            if (!_activityService.ActivityExists(activityId))
            {
                return NotFound();
            }

            var result = await _activityService.DeleteActivity(activityId);

            if (result.ResponseError == null)
            {
                return NoContent();
            }

            return StatusCode(500);
        }

    }
}
