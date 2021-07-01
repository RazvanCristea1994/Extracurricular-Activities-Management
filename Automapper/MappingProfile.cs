using AutoMapper;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels;
using ExtracurricularActivitiesManagement.ViewModels.Activity;
using ExtracurricularActivitiesManagement.ViewModels.Authentication;
using ExtracurricularActivitiesManagement.ViewModels.Booking;
using ExtracurricularActivitiesManagement.ViewModels.ScheduledActivitya;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityViewModel>().ReverseMap();
            CreateMap<Activity, ActivityWithTeachersViewModel>().ReverseMap();

            CreateMap<ApplicationUser, AuthenticationUserResponse>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();

            CreateMap<ScheduledActivity, ScheduledActivityViewModel>().ReverseMap();
            CreateMap<Booking, BookingViewModel>();

            CreateMap<Teacher, TeacherViewModel>().ReverseMap();

        }
    }
}
