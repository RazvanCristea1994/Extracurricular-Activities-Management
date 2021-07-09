using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using ExtracurricularActivitiesManagement.ViewModels.ScheduledActivitya;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services
{
    public class ScheduledActivityService : IScheduledActivity
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public ScheduledActivityService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ScheduledActivityViewModel, IEnumerable<ManagementError>>> CreateScheduledActivity(ScheduledActivity scheduledActivity)
        {
            var activity = _context.Activities.Find(scheduledActivity.ActivityId);
            var teacher = _context.Teachers.Find(scheduledActivity.TeacherId);
            scheduledActivity.Teacher = teacher;
            scheduledActivity.Activity = activity;

            _context.ScheduledActivities.Add(scheduledActivity);

            var serviceResponse = new ServiceResponse<ScheduledActivityViewModel, IEnumerable<ManagementError>>();
            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = _mapper.Map<ScheduledActivityViewModel>(scheduledActivity);
            }
            catch (Exception e)
            {
                var errors = new List<ManagementError>();
                errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<PaginatedResultSet<ScheduledActivityViewModel>, IEnumerable<ManagementError>>> GetScheduledActivities(int? page = 1, int? perPage = 10)
        {
            var scheduledActivities = await _context.ScheduledActivities
                .Include(s => s.Activity)
                .Include(s => s.Teacher)
                .Skip((page.Value - 1) * perPage.Value)
                .Take(perPage.Value)
                .OrderByDescending(t => t.StartTime)
                .ToListAsync();

            var scheduledActivitiesResult = _mapper.Map<List<ScheduledActivity>, List<ScheduledActivityViewModel>>(scheduledActivities);

            var count = _context.ScheduledActivities.Count();

            var result = new PaginatedResultSet<ScheduledActivityViewModel>(scheduledActivitiesResult, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<ScheduledActivityViewModel>, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }

        public async Task<ServiceResponse<ScheduledActivityViewModel, IEnumerable<ManagementError>>> GetScheduledActivity(int id)
        {
            var scheduledActivity = await _context.ScheduledActivities.FindAsync(id);

            var result = _mapper.Map<ScheduledActivityViewModel>(scheduledActivity);

            var serviceResponse = new ServiceResponse<ScheduledActivityViewModel, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> UpdateScheduledActivity(ScheduledActivity scheduledActivity)
        {
            _context.Entry(scheduledActivity).State = EntityState.Modified;
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

        public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteScheduledActivity(int scheduledActivityId)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();

            try
            {
                var scheduledActivity = await _context.ScheduledActivities.FindAsync(scheduledActivityId);
                _context.ScheduledActivities.Remove(scheduledActivity);
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

        public bool ScheduledActivityExists(int scheduledActivityId)
        {
            return _context.ScheduledActivities.Any(s => s.Id == scheduledActivityId);
        }
    }
}
