using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public ScheduledActivity ScheduledActivity { get; set; }
        public int ScheduledActivityId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
