using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.ScheduledActivitya;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledActivitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ScheduledActivitiesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ScheduledActivityViewModel>> PostScheduledActivity(ScheduledActivityViewModel scheduledActivity)
        {
            var entity = _mapper.Map<ScheduledActivity>(scheduledActivity);
            var activity = _context.Activities.Find(entity.ActivityId);
            var teacher = _context.Teachers.Find(entity.TeacherId);
            entity.Teacher = teacher;
            entity.Activity = activity;

            _context.ScheduledActivities.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScheduledActivity", new { id = entity.Id }, _mapper.Map<ScheduledActivityViewModel>(entity));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledActivity>>> GetScheduledActivities()
        {
            return await _context.ScheduledActivities
                .Include(a => a.Activity)
                .Include(a => a.Teacher)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduledActivity>> GetScheduledActivity(int id)
        {
            var scheduledActivity = await _context.ScheduledActivities.FindAsync(id);

            if (scheduledActivity == null)
            {
                return NotFound();
            }

            return scheduledActivity;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutScheduledActivity(int id, ScheduledActivity scheduledActivity)
        {
            if (id != scheduledActivity.Id)
            {
                return BadRequest();
            }

            _context.Entry(scheduledActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduledActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduledActivity(int id)
        {
            var scheduledActivity = await _context.ScheduledActivities.FindAsync(id);
            if (scheduledActivity == null)
            {
                return NotFound();
            }

            _context.ScheduledActivities.Remove(scheduledActivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScheduledActivityExists(int id)
        {
            return _context.ScheduledActivities.Any(e => e.Id == id);
        }
    }
}
