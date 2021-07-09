using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services.ServicesInterfaces
{
    public interface IActivityService
    {
		public Task<ServiceResponse<ActivityViewModel, IEnumerable<ManagementError>>> CreateActivity(Activity activity);
		public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> AddActivityToTeacher(int teacherId, Activity activity);

		public Task<ServiceResponse<List<ActivityViewModel>, IEnumerable<ManagementError>>> GetActivities();
		public Task<ServiceResponse<ActivityViewModel, IEnumerable<ManagementError>>> GetActivity(int id);

		public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> UpdateActivity(Activity activity);

		public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> RemoveActivityFromTeacher(int storyId, int activityId);
		public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteActivity(int activityId);

		public bool ActivityExists(int activityId);
	}
}
