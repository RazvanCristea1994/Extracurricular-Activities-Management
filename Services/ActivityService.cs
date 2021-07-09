using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Activity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services
{
    public class ActivityService : IActivityService
    {
		private ApplicationDbContext _context;
		private IMapper _mapper;

		public ActivityService(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<ActivityViewModel, IEnumerable<ManagementError>>> CreateActivity(Activity activity)
		{
			_context.Activities.Add(activity);
			var serviceResponse = new ServiceResponse<ActivityViewModel, IEnumerable<ManagementError>>();

			try
			{
				await _context.SaveChangesAsync();
				serviceResponse.ResponseOk = _mapper.Map<ActivityViewModel>(activity);
			}
			catch (Exception e)
			{
				var errors = new List<ManagementError>();
				errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
				serviceResponse.ResponseError = errors;
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> AddActivityToTeacher(int teacherId, Activity activity)
		{
			var teacher = await _context.Teachers
				.Include(t => t.Activities)
				.Where(t => t.Id == teacherId)
				.FirstOrDefaultAsync();

			var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();

			if (teacher == null)
			{
				var errors = new List<ManagementError>();
				errors.Add(new ManagementError { Description = "The teacher doesn't exist." });
				serviceResponse.ResponseError = errors;
				return serviceResponse;
			}

			try
			{
				if (!teacher.Activities.Contains(activity))
				{
					teacher.Activities.Add(activity);
					_context.Entry(teacher).State = EntityState.Modified;
					_context.SaveChanges();
				}
				serviceResponse.ResponseOk = true;
			}
			catch (Exception e)
			{
				var errors = new List<ManagementError>();
				errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<List<ActivityViewModel>, IEnumerable<ManagementError>>> GetActivities()
		{
			var activities = await _context.Activities
				.ToListAsync();

			var resultActivities = _mapper.Map<List<Activity>, List<ActivityViewModel>>(activities);

			var serviceResponse = new ServiceResponse<List<ActivityViewModel>, IEnumerable<ManagementError>>();
			serviceResponse.ResponseOk = resultActivities;
			return serviceResponse;
		}

		public async Task<ServiceResponse<ActivityViewModel, IEnumerable<ManagementError>>> GetActivity(int id)
		{
			var activity = await _context.Activities.FindAsync(id);

			var activityResult = _mapper.Map<ActivityViewModel>(activity);

			var serviceResponse = new ServiceResponse<ActivityViewModel, IEnumerable<ManagementError>>();
			serviceResponse.ResponseOk = activityResult;
			return serviceResponse;
		}

		public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> UpdateActivity(Activity activity)
		{
			_context.Entry(activity).State = EntityState.Modified;
			var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();

			try
			{
				await _context.SaveChangesAsync();
				serviceResponse.ResponseOk = true;
			}
			catch (DbUpdateConcurrencyException e)
			{
				var errors = new List<ManagementError>();
				errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
				serviceResponse.ResponseError = errors;
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> RemoveActivityFromTeacher(int teacherId, int activityId)
		{
			var teacher = await _context.Teachers
							.Include(t => t.Activities)
							.Where(t => t.Id == teacherId)
							.FirstOrDefaultAsync();

			var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();

			if (teacher == null)
			{
				var errors = new List<ManagementError>();
				errors.Add(new ManagementError { Description = "The teacher doesn't exist." });
				serviceResponse.ResponseError = errors;
				return serviceResponse;
			}

			try
			{
				var activity = await _context.Activities.FindAsync(activityId);
				teacher.Activities.Remove(activity);

				_context.Entry(teacher).State = EntityState.Modified;
				_context.SaveChanges();
				serviceResponse.ResponseOk = true;
			}
			catch (Exception e)
			{
				var errors = new List<ManagementError>();
				errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteActivity(int activityId)
		{
			var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();

			try
			{
				var activity = await _context.Activities.FindAsync(activityId);
				_context.Activities.Remove(activity);
				await _context.SaveChangesAsync();
				serviceResponse.ResponseOk = true;
			}
			catch (Exception e)
			{
				var errors = new List<ManagementError>();
				errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
				serviceResponse.ResponseError = errors;
			}

			return serviceResponse;
		}

		public bool ActivityExists(int activityId)
		{
			return _context.Activities.Any(e => e.Id == activityId);
		}
	}
}
