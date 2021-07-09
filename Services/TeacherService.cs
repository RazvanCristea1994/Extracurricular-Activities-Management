using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Activity;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services
{
    public class TeacherService : ITeacherService
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private IActivityService _activityService;

        public TeacherService(ApplicationDbContext context, IMapper mapper, IActivityService activityService)
        {
            _context = context;
            _mapper = mapper;
            _activityService = activityService;
        }

        public async Task<ServiceResponse<TeacherViewModel, IEnumerable<ManagementError>>> CreateTeacher(Teacher teacher)
        {
            var activities = teacher.Activities is List<Activity> ? teacher.Activities : new List<Activity>();
            teacher.Activities = null;

            _context.Teachers.Add(teacher);

            var serviceResponse = new ServiceResponse<TeacherViewModel, IEnumerable<ManagementError>>();

            try
            {
                await _context.SaveChangesAsync();
                activities.ForEach(
                    async t => await _activityService.AddActivityToTeacher(teacher.Id, new Activity { Id = t.Id, Name = t.Name })
                );

                serviceResponse.ResponseOk = _mapper.Map<TeacherViewModel>(teacher);
            }
            catch (Exception e)
            {
                var errors = new List<ManagementError>();
                errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<PaginatedResultSet<TeacherViewModel>, IEnumerable<ManagementError>>> GetTeachers(int? page = 1, int? perPage = 10)
        {
            var teachers = await _context.Teachers
                .Include(t => t.Activities)
                .Skip((page.Value - 1) * perPage.Value)
                .Take(perPage.Value)
                .OrderByDescending(t => t.Id)
                .ToListAsync();

            var teachersResult = _mapper.Map<List<Teacher>, List<TeacherViewModel>>(teachers);

            var count = _context.Teachers.Count();

            var result = new PaginatedResultSet<TeacherViewModel>(teachersResult, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<TeacherViewModel>, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }

        public async Task<ServiceResponse<TeacherViewModel, IEnumerable<ManagementError>>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers
                .Where(t => t.Id == id)
                .Include(t => t.Activities)
                .FirstOrDefaultAsync();

            var serviceResponse = new ServiceResponse<TeacherViewModel, IEnumerable<ManagementError>>();
            var result = _mapper.Map<TeacherViewModel>(teacher);

            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }

        public async Task<ServiceResponse<PaginatedResultSet<TeacherViewModel>, IEnumerable<ManagementError>>> GetFilteredTeachers(int activityId, int? page = 1, int? perPage = 10)
        {
            var activity = await _context.Activities.FindAsync(activityId);

            var teachers = await _context.Teachers
                .Include(t => t.Activities)
                .Where(t => t.Activities.Contains(activity))
                .Skip((page.Value - 1) * perPage.Value)
                .Take(perPage.Value)
                .OrderByDescending(s => s.Id)
                .ToListAsync();

            var teachersResult = _mapper.Map<List<TeacherViewModel>>(teachers);

            var count = _context.Teachers.Where(t => t.Activities.Contains(activity)).Count();

            var result = new PaginatedResultSet<TeacherViewModel>(teachersResult, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<TeacherViewModel>, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> UpdateTeacher(Teacher teacher)
        {
            var loadedTeacher = await _context.Teachers.Where(t => t.Id == teacher.Id)
                .Include(s => s.Activities)
                .FirstOrDefaultAsync();

            loadedTeacher.Activities.RemoveAll(t => t.Id > 0);
            teacher.Activities.ForEach(t => {
                var activity = _context.Activities.Find(t.Id);
                loadedTeacher.Activities.Add(activity);
            });

            loadedTeacher.Description = teacher.Description;
            loadedTeacher.FirstName = teacher.FirstName;
            loadedTeacher.LastName = teacher.LastName;

            var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();
            _context.Entry(loadedTeacher).State = EntityState.Modified;

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

        public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteTeacher(int teacherId)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();

            try
            {
                var teacher = await _context.Teachers.FindAsync(teacherId);
                _context.Teachers.Remove(teacher);
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

        public bool TeacherExists(int teacherId)
        {
            return _context.Teachers.Any(e => e.Id == teacherId);
        }
    }
}
