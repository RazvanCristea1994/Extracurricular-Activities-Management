using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.UiValidators
{
    public class TeacherValidation : AbstractValidator<TeachersWithActivitiesViewModel>
    {
        private readonly ApplicationDbContext _context;

        public TeacherValidation(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(t => t.FirstName).NotNull().NotEmpty().Matches(@"^\S*$").Length(1, 20).WithMessage("The first name should have between 4 and 20 characters.");
            RuleFor(t => t.LastName).NotNull().NotEmpty().Matches(@"^\S*$").Length(1, 20).WithMessage("The last name should have between 4 and 20 characters.");
            RuleFor(t => t.Description).NotNull().NotEmpty().Matches(@"^\S*").Length(1, 50).WithMessage("The description should have between 4 and 50 characters.");
        }
    }
}
