using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.ViewModels
{
    public enum GenderType
    {
        [Display(Name = "Female")]
        FEMALE,
        [Display(Name = "Male")]
        MALE
    }
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PlainPassword { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderType Gender { get; set; }
    }
}
