using ExtracurricularActivitiesManagement.ViewModels.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.ViewModels.TeacherViews
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Description { get; set; }
        public List<ActivityViewModel> Activities { get; set; }
    }
}
