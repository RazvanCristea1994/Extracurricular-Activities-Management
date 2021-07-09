using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.Booking;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services.ServicesInterfaces
{
    public interface IBookingService
    {
        public Task<ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>> BookSpot(ScheduledActivity scheduledActivity, ApplicationUser user);

        public Task<ServiceResponse<PaginatedResultSet<BookingViewModel>, IEnumerable<ManagementError>>> GetBookings(int? page = 1, int? perPage = 10);
        public Task<ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>> GetBooking(int id);
        public Task<ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>> GetUserBooking(int scheduledActivityId, ApplicationUser user);
        public Task<ServiceResponse<PaginatedResultSet<BookingViewModel>, IEnumerable<ManagementError>>> GetBookingsForCurrentUser(ApplicationUser user, int? page = 1, int? perPage = 10);

        public Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteBooking(int id, ApplicationUser user);

        public bool BookingExists(int bookingId);
    }
}
