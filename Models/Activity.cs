using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Models
{
    public enum ActivityType
    {
        Football,
        Volleyball,
        Basketball,
        Tenis,
        Rugby
    }
    public class Activity
    {
        public int Id { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; }
        public string ActivityPicture { get; set; }
        public string PrimaryColour { get; set; }
        public string SecondaryColour { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
    }
}
