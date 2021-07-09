using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
using System.Collections.Generic;

namespace ExtracurricularActivitiesManagement.ViewModels.Activity
{
    public class ActivityWithTeachersViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActivityPicture { get; set; }
        public string PrimaryColour { get; set; }
        public string SecondaryColour { get; set; }
        public IEnumerable<TeacherViewModel> Teachers { get; set; }
    }
}
