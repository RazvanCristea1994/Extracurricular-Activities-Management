using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
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
    public class TeachersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITeacherService _teacherService;
		private readonly UserManager<ApplicationUser> _userManager;

		public TeachersController(IMapper mapper, ITeacherService teacherManagementService, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _teacherService = teacherManagementService;
			_userManager = userManager;
        }

		[HttpPost]
		[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
		public async Task<ActionResult<Teacher>> PostTeacher(TeacherViewModel teacher)
		{
			var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			if (!await _userManager.IsInRoleAsync(user, UserRole.ROLE_ADMIN))
			{
				return StatusCode(403);
			}

			var teacherEntity = _mapper.Map<Teacher>(teacher);
			
			var teacherResponse = await _teacherService.CreateTeacher(teacherEntity);

			if (teacherResponse.ResponseError == null)
			{
				return CreatedAtAction("GetTeacher", new { id = teacherEntity.Id }, teacher);
			}

			return StatusCode(500);
		}

		[HttpGet]
		public async Task<ActionResult<PaginatedResultSet<TeacherViewModel>>> GetTeachers()
		{
			var result = await _teacherService.GetTeachers();
			return result.ResponseOk;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TeacherViewModel>> GetTeacher(int id)
		{
			var response = await _teacherService.GetTeacher(id);
			var teacher = response.ResponseOk;

			if (teacher == null)
			{
				return NotFound();
			}

			return teacher;
		}

		[HttpGet]
		[Route("filter/{activityId}")]
		public async Task<ActionResult<PaginatedResultSet<TeacherViewModel>>> GetFilteredTeachers(int activityId, int? page = 1, int? perPage = 10)
		{
			var result = await _teacherService.GetFilteredTeachers(activityId, page, perPage);
			return result.ResponseOk;
		}

		[HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
		public async Task<IActionResult> PutTeacher(int id, TeacherViewModel teacher)
		{
			if (id != teacher.Id)
			{
				return BadRequest();
			}

			var response = await _teacherService.UpdateTeacher(_mapper.Map<Teacher>(teacher));

			if (response.ResponseError == null)
			{
				return NoContent();
			}

			if (!_teacherService.TeacherExists(id))
			{
				return NotFound();
			}

			return StatusCode(500);
		}

		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
		public async Task<IActionResult> DeleteTeacher(int id)
		{
			if (!_teacherService.TeacherExists(id))
			{
				return NotFound();
			}

			var result = await _teacherService.DeleteTeacher(id);

			if (result.ResponseError == null)
			{
				return NoContent();
			}

			return StatusCode(500);
		}
	}
}
