using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Models
{
    public class ScheduledActivity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Activity Activity { get; set; }
        public int ActivityId { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        private DateTime startDate;
        public DateTime StartTime { get => startDate; set => startDate = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        public DateTime endDate;
        public DateTime EndTime { get => endDate; set => endDate = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        public int Capacity { get; set; }
    }
}
