using AutoMapper;
using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.ViewModels.TeacherViews;
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
    public class TeachersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TeachersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTrainer(TeachersWithActivitiesViewModel trainer)
        {
            var trainerEntity = _mapper.Map<Teacher>(new Teacher());
            trainerEntity.Id = trainer.Id;
            trainerEntity.FirstName = trainer.FirstName;
            trainerEntity.LastName = trainer.LastName;
            trainerEntity.Description = trainer.Description;

            if (trainer.Activities.Count != 0)
            {
                List<Activity> activities = new List<Activity>();
                trainer.Activities.ForEach(activityId =>
                {
                    var activity = _context.Activities.Find(activityId);
                    if (activity != null)
                    {
                        activities.Add(activity);
                    }
                });

                if (activities.Count == 0)
                {
                    return BadRequest("The activities you provided are not available.");
                }
                trainerEntity.Activities = activities;
            }

            _context.Teachers.Add(trainerEntity);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTrainer", new { id = trainer.Id }, trainer);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherViewModel>>> GetTrainers()
        {
            return await _context.Teachers
                .OrderBy(t => t.LastName)
                .Include(t => t.Activities)
                .Select(t => _mapper.Map<TeacherViewModel>(t))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherViewModel>> GetTrainer(int id)
        {
            var trainer = await _context.Teachers.Include(t => t.Activities).FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
            {
                return NotFound();
            }

            return _mapper.Map<TeacherViewModel>(trainer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainer(TeachersWithActivitiesViewModel trainerFromUi)
        {
            var trainerToUpdate = _context.Teachers
                .Include(t => t.Activities)
                .FirstOrDefault(t => t.Id == trainerFromUi.Id);

            if (trainerToUpdate == null)
            {
                return NotFound();
            }

            if (trainerFromUi.Activities.Count != 0)
            {
                var activitiesToRemove = trainerToUpdate.Activities.ToList();
                activitiesToRemove.ForEach(activity =>
                {
                    if (!trainerFromUi.Activities.Contains(activity.Id))
                    {
                        trainerToUpdate.Activities.Remove(activity);
                    }
                });
                trainerFromUi.Activities.ForEach(activityId =>
                {
                    var activityToAdd = _context.Activities.Find(activityId);
                    if (activityToAdd != null && !trainerToUpdate.Activities.Exists(a => a.Id == activityToAdd.Id))
                    {
                        trainerToUpdate.Activities.Add(activityToAdd);
                    }
                });
            }
            else
            {
                trainerToUpdate.Activities.Clear();
            }
            var newActivities = trainerToUpdate.Activities;

            trainerToUpdate.Id = trainerFromUi.Id;
            trainerToUpdate.FirstName = trainerFromUi.FirstName;
            trainerToUpdate.LastName = trainerFromUi.LastName;
            trainerToUpdate.Description = trainerFromUi.Description;
            trainerToUpdate.Activities = newActivities;

            _context.Entry(trainerToUpdate).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Teachers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(trainer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainerExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
