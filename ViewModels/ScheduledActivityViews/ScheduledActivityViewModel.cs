using ExtracurricularActivitiesManagement.ViewModels.Activity;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.ViewModels.ScheduledActivitya
{
    public class ScheduledActivityViewModel
    {
		public int Id { get; set; }
		public string Description { get; set; }
		public ActivityViewModel Activity { get; set; }
		public int ActivityId { get; set; }
		public TeacherViewModel Teacher { get; set; }
		public int TeacherId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int TimezoneOffsetMinutes { get; set; }
		public int Capacity { get; set; }
	}
}
