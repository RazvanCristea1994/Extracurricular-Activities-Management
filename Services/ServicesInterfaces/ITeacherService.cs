using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services.ServicesInterfaces
{
    public interface ITeacherService
    {
        public Task<ServiceResponse<TeacherViewModel, IEnumerable<ManagementError>>> CreateTeacher(Teacher teacher);

        public Task<ServiceResponse<PaginatedResultSet<TeacherViewModel>, IEnumerable<ManagementError>>> GetTeachers(int? page = 1, int? perPage = 10);
        public Task<ServiceResponse<TeacherViewModel, IEnumerable<ManagementError>>> GetTeacher(int id);
        public Task<ServiceResponse<PaginatedResultSet<TeacherViewModel>, IEnumerable<ManagementError>>> GetFilteredTeachers(int activityId, int? page = 1, int? perPage = 10);
       
        public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> UpdateTeacher(Teacher teacher);

        public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteTeacher(int teacherId);
        
        public bool TeacherExists(int teacherId);
    }
}
