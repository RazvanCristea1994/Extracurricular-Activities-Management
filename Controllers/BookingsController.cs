using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("BookSpot")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<Booking>> BookSpot(ScheduledActivity activity)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var booking = new Booking { ScheduledActivityId = activity.Id, UserId = user.Id };
            _context.Bookings.Add(booking);
            activity.Capacity--;

            _context.Entry(activity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpGet("ScheduledActivity/{scheduledActivityId}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<Booking>> GetUserBooking(int scheduledActivityId)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var booking = await _context.Bookings
                .Where(b => b.UserId == user.Id && b.ScheduledActivityId == scheduledActivityId)
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                return new EmptyResult();
            }

            return booking;
        }

        [HttpGet("/Bookings/Current")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsForCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user == null)
            {
                return new List<Booking>();
            }

            return await _context.Bookings
                .Where(b => b.UserId == user.Id)
                .ToListAsync();
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            var activity = _context.ScheduledActivities.Find(booking.ScheduledActivityId);
            activity.Capacity++;
            _context.Entry(activity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
