using ExtracurricularActivitiesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.ViewModels.Booking
{
	public class BookingViewModel
	{
		public int Id { get; set; }
		public ScheduledActivity ScheduledActivity { get; set; }
		public int ScheduledActivityId { get; set; }
		public ApplicationUser User { get; set; }
		public int UserId { get; set; }
	}
}
