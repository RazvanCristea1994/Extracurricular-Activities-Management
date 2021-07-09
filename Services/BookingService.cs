using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Errors;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Booking;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services
{
    public class BookingService : IBookingService
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public BookingService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>> BookSpot(ScheduledActivity scheduledActivity, ApplicationUser user)
        {
            var booking = new Booking { ScheduledActivityId = scheduledActivity.Id, UserId = user.Id };

            _context.Bookings.Add(booking);
            scheduledActivity.Capacity--;
            _context.Entry(scheduledActivity).State = EntityState.Modified;

            var serviceResponse = new ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>();
            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = _mapper.Map<BookingViewModel>(booking);
            }
            catch (Exception e)
            {
                var errors = new List<ManagementError>();
                errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<PaginatedResultSet<BookingViewModel>, IEnumerable<ManagementError>>> GetBookings(int? page = 1, int? perPage = 10)
        {
            var bookings = await _context.Bookings.ToListAsync();

            var bookingsResult = _mapper.Map<List<Booking>, List<BookingViewModel>>(bookings);

            var count = _context.Bookings.Count();

            var result = new PaginatedResultSet<BookingViewModel>(bookingsResult, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<BookingViewModel>, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }
        public async Task<ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            var result = _mapper.Map<BookingViewModel>(booking);

            var serviceResponse = new ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }
        public async Task<ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>> GetUserBooking(int scheduledActivityId, ApplicationUser user)
        {
            var booking = await _context.Bookings
                    .Where(b => b.UserId == user.Id && b.ScheduledActivityId == scheduledActivityId)
                    .FirstOrDefaultAsync();

            var result = _mapper.Map<BookingViewModel>(booking);

            var serviceResponse = new ServiceResponse<BookingViewModel, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }
        public async Task<ServiceResponse<PaginatedResultSet<BookingViewModel>, IEnumerable<ManagementError>>> GetBookingsForCurrentUser(ApplicationUser user, int? page = 1, int? perPage = 10)
        {
            var bookings = await _context.Bookings
               .Where(s => s.UserId == user.Id)
               .ToListAsync();

            var bookingsResult = _mapper.Map<List<Booking>, List<BookingViewModel>>(bookings);

            var count = _context.Bookings.Count();

            var result = new PaginatedResultSet<BookingViewModel>(bookingsResult, page.Value, count, perPage.Value);

            var serviceResponse = new ServiceResponse<PaginatedResultSet<BookingViewModel>, IEnumerable<ManagementError>>();
            serviceResponse.ResponseOk = result;
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<ManagementError>>> DeleteBooking(int bookingId, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<ManagementError>>();

            try
            {
                var booking = await _context.ScheduledActivities.FindAsync(bookingId);
                _context.ScheduledActivities.Remove(booking);
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = true;
            }
            catch (Exception e)
            {
                var errors = new List<ManagementError>();
                errors.Add(new ManagementError { Code = e.GetType().ToString(), Description = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public bool BookingExists(int bookingId)
        {
            return _context.Bookings.Any(b => b.Id == bookingId);
        }
    }
}
