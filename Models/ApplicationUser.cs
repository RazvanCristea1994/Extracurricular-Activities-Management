using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GenderType
    {
        FEMALE,
        MALE,
    }
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
