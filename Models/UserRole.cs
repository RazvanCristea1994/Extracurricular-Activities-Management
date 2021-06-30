using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Models
{
    public class UserRole : IdentityRole
    {
        public const string ROLE_ADMIN = "AppAdmin";
        public const string ROLE_STUDENT = "AppStudent";
    }
}
