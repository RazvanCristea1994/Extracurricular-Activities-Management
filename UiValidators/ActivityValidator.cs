using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.Activity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.UiValidators
{
    public class ActivityValidator : AbstractValidator<ActivityViewModel>
    {
        private readonly ApplicationDbContext _context;

        public ActivityValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(m => m.Description).MinimumLength(10).MaximumLength(150).WithMessage($"The description must be between 10 and 150 characters long.");
            RuleFor(m => m.ActivityPicture).MinimumLength(4).When(m => !string.IsNullOrEmpty(m.ActivityPicture));
            RuleFor(m => m.PrimaryColour).Must(IsAValidHexCode);
            RuleFor(m => m.SecondaryColour).Must(IsAValidHexCode);
        }

        private bool IsAValidHexCode(string val)
        {
            int res = 0;
            if (int.TryParse(val,
                     System.Globalization.NumberStyles.HexNumber,
                     System.Globalization.CultureInfo.InvariantCulture, out res))
                return true;
            else
                return false;
        }
    }
}
