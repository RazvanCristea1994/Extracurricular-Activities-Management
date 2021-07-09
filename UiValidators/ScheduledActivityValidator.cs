using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.ViewModels.ScheduledActivitya;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.UiValidators
{
    public class ScheduledActivityValidator : AbstractValidator<ScheduledActivityViewModel>
	{
		private readonly ApplicationDbContext _context;

		public ScheduledActivityValidator(ApplicationDbContext context)
		{
			_context = context;

			RuleFor(m => m.Capacity).GreaterThan(0);
			RuleFor(m => m.ActivityId).NotNull().WithMessage("You must specify an activity.");
			RuleFor(m => m.ActivityId).NotEqual(0).WithMessage("You must specify an activity.");
			RuleFor(m => m.TeacherId).NotNull().WithMessage("You must specify a trainer.");
			RuleFor(m => m.TeacherId).NotEqual(0).WithMessage("You must specify a trainer.");
			RuleFor(m => m.StartTime).NotNull();
			RuleFor(m => m.EndTime).NotNull();
			RuleFor(m => m.StartTime).LessThan(m => m.EndTime).WithMessage("The start time must be earlier than the end time.");
			RuleFor(m => m.StartTime).GreaterThan(DateTime.UtcNow).WithMessage("The start time must be in the future.");
			RuleFor(m => m.StartTime.AddMinutes(-m.TimezoneOffsetMinutes)).Must(StartWithinDayTime).WithMessage("Events must start within day time (9-20).");
			RuleFor(m => m.EndTime.AddMinutes(-m.TimezoneOffsetMinutes)).Must(EndWithinDayTime).WithMessage("Events must end within day time (9-20).");
			RuleFor(m => new { m.StartTime, m.EndTime }).Must(x => !HaveOverlappingEvents(x.StartTime, x.EndTime)).WithMessage("This event overlaps with another one.");
		}

		private bool StartWithinDayTime(DateTime datetime)
		{
			return datetime.Hour >= 9 && datetime.Hour < 20;
		}

		private bool EndWithinDayTime(DateTime datetime)
		{
			return datetime.Hour >= 9 && datetime.Hour < 21;
		}

		private bool HaveOverlappingEvents(DateTime startTime, DateTime endTime)
		{
			var overlappingActivitiesCount = _context.ScheduledActivities
				.Where(a => a.StartTime >= startTime && a.StartTime <= endTime || a.EndTime >= startTime && a.EndTime <= endTime)
				.Count();

			return overlappingActivitiesCount > 0;
		}
	}
}
