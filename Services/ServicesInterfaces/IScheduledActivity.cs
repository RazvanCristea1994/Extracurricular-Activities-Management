using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using ExtracurricularActivitiesManagement.ViewModels.ScheduledActivitya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services.ServicesInterfaces
{
    public interface IScheduledActivity
    {
        public Task<ServiceResponse<ScheduledActivityViewModel, IEnumerable<ManagementError>>> CreateScheduledActivity(ScheduledActivity scheduledActivity);

        public Task<ServiceResponse<PaginatedResultSet<ScheduledActivityViewModel>, IEnumerable<ManagementError>>> GetScheduledActivities(int? page = 1, int? perPage = 10);
        public Task<ServiceResponse<ScheduledActivityViewModel, IEnumerable<ManagementError>>> GetScheduledActivity(int id);

        public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> UpdateScheduledActivity(ScheduledActivity scheduledActivity);

        public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteScheduledActivity(int id);

        public bool ScheduledActivityExists(int scheduledActivityid);
    }
}
