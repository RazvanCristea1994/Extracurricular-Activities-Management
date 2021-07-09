using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Booking;
using ExtracurricularActivitiesManagement.ViewModels.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBookingService _bookingService;
        private IMapper _mapper;
        private readonly IScheduledActivity _scheduledActivityService;

        public BookingsController(UserManager<ApplicationUser> userManager, IBookingService bookingService, IMapper mapper, IScheduledActivity scheduledActivityService)
        {
            _userManager = userManager;
            _bookingService = bookingService;
            _mapper = mapper;
            _scheduledActivityService = scheduledActivityService;
        }

        [HttpPost("BookSpot")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<BookingViewModel>> BookSpot(ScheduledActivity scheduledActivity)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
           
            var response = await _bookingService.BookSpot(scheduledActivity, user);

            if (response.ResponseError == null)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("bookings")]
        public async Task<ActionResult<PaginatedResultSet<BookingViewModel>>> GetBookings()
        {
            var result = await _bookingService.GetBookings();
            return result.ResponseOk;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingViewModel>> GetBooking(int id)
        {
            var response = await _bookingService.GetBooking(id);
            var booking = response.ResponseOk;

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpGet("ScheduledActivity/{scheduledActivityId}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<BookingViewModel>> GetUserBooking(int scheduledActivityId)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var response = await _bookingService.GetUserBooking(scheduledActivityId, user);
            var booking = response.ResponseOk;

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpGet("/Bookings/Current")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<PaginatedResultSet<BookingViewModel>>> GetBookingsForCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var response = await _bookingService.GetBookingsForCurrentUser(user);
            var booking = response.ResponseOk;

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!_bookingService.BookingExists(id))
            {
                return NotFound();
            }

            var result = await _bookingService.DeleteBooking(id, user);

            if (result.ResponseError == null)
            {
                return NoContent();
            }

            return StatusCode(500);
        }

    }
}
