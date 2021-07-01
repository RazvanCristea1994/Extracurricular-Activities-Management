using ExtracurricularActivitiesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.ViewModels.Activity
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public ActivityType Type { get; set; }
        public string Description { get; set; }
        public string ActivityPicture { get; set; }
        public string PrimaryColour { get; set; }
        public string SecondaryColour { get; set; }
    }
}
