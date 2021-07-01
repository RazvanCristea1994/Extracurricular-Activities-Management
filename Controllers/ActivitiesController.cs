using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.Activity;
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
    public class ActivitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActivitiesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(ActivityViewModel activityRequest)
        {
            Activity activity = _mapper.Map<Activity>(activityRequest);
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.Id }, activity);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityWithTeachersViewModel>>> GetActivities()
        {
            return await _context.Activities
                            .Include(a => a.Teachers)
                            .Select(a => _mapper.Map<ActivityWithTeachersViewModel>(a)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityViewModel>> GetActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }
            return _mapper.Map<ActivityViewModel>(activity);
        }

        [HttpGet]
        [Route("filterByType/{activityType}")]
        public async Task<ActionResult<IEnumerable<ActivityViewModel>>> FilterByType(string activityType)
        {
            return await _context.Activities
                .Where(a => a.ActivityType.Equals(activityType))
                .Select(a => _mapper.Map<ActivityViewModel>(a))
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, ActivityViewModel activity)
        {
            if (id != activity.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<Activity>(activity)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
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

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(a => a.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
