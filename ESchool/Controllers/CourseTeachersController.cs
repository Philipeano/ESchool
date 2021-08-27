using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ESchool.Models;

namespace ESchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseTeachersController : ControllerBase
    {
        private readonly ESchoolContext _context;

        public CourseTeachersController(ESchoolContext context)
        {
            _context = context;
        }


        // Get all course teachers
        // GET: api/CourseTeachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseTeacher>>> GetCourseTeachers()
        {
            return await _context.CourseTeachers
                .Include(ct => ct.AssignedCourse)
                .Include(ct => ct.AssignedTeacher)
                .ToListAsync();
        }

        // Get a specific teacher that teaches a specific course
        // GET: api/CourseTeachers?CourseId={courseId}&TeacherId={teacherId}
        [HttpGet("CourseId={courseId}&TeacherId={teacherId}")]
        public async Task<ActionResult<CourseTeacher>> GetSpecificCourseTeacher([FromQuery] long courseId, [FromQuery] long teacherId)
        {
            return await _context.CourseTeachers
                .Where(ct => ct.CourseId == courseId && ct.TeacherId == teacherId)
                .Include(ct => ct.AssignedCourse)
                .Include(ct => ct.AssignedTeacher)
                .FirstOrDefaultAsync();
        }

        // Assign a specific teacher to teach a specific course
        // POST: api/CourseTeachers
        [HttpPost]
        public async Task<IActionResult> PostCourseTeacher([FromBody] CourseTeacher courseTeacher)
        {
            // Obtain a reference to the specified teacher
            var teacher = await _context.Teachers.FindAsync(courseTeacher.TeacherId);

            // Obtain a reference to the specified course
            var course = await _context.Courses.FindAsync(courseTeacher.CourseId);

            // Create a new courseTeacher object
            var newCourseTeacher = new CourseTeacher()
            {
                AssignedCourse = course,
                AssignedTeacher = teacher
            };

            _context.CourseTeachers.Add(newCourseTeacher);
            await _context.SaveChangesAsync();

            return Created($"api/CourseTeachers?CourseId={courseTeacher.CourseId}&TeacherId={courseTeacher.TeacherId}", newCourseTeacher);
        }

        // These two endpoints can also be used to assign a course to a teacher and vice-versa 
        // POST: api/Courses/{courseId}/Teachers
        // POST: api/Teachers/{teacherId}/Courses



        // Remove a specific teacher from teaching a specific course
        // DELETE: api/CourseTeachers?CourseId={courseId}&TeacherId={teacherId}
        [HttpDelete]
        public async Task<ActionResult<CourseTeacher>> DeleteCourseTeacher([FromQuery] long courseId, [FromQuery] long teacherId)
        {
            var courseTeacher = await _context.CourseTeachers
                .Where(ct => ct.CourseId == courseId && ct.TeacherId==teacherId)
                .FirstOrDefaultAsync();

            if (courseTeacher == null)
            {
                return NotFound();
            }

            _context.CourseTeachers.Remove(courseTeacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }




        // The following endpoint should be implemented in the CoursesController
        // GET: api/Courses/{id}/Teachers

    }
}
