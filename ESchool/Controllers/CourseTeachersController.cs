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


        // Get all course teachers, optionally filtered by courseId and/or teacherId
        // GET: api/CourseTeachers?CourseId={courseId}&TeacherId={teacherId}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseTeacher>>> GetCourseTeachers([FromQuery(Name = "courseId")] long courseId, [FromQuery(Name = "teacherId")] long teacherId)
        {
            // Define a default query to fetch all course teachers unfiltered
            IQueryable<CourseTeacher> query = _context.CourseTeachers
                .Include(ct => ct.AssignedCourse)
                .Include(ct => ct.AssignedTeacher);

            // If no URL parameters were specified for this request, return all query data 
            if (Request.Query.Count == 0)
                return await query.ToListAsync();

            // If both URL parameters were provided, apply both filters to the query data
            if (Request.Query.Count == 2 && Request.Query.ContainsKey("courseId") && Request.Query.ContainsKey("teacherId"))
                return await query.Where(ct => ct.CourseId == courseId && ct.TeacherId == teacherId).ToListAsync();

            // If only one of the URL parameters was provided, apply just the specified filter  
            if (Request.Query.Count == 1 && Request.Query.ContainsKey("courseId"))
                return await query.Where(ct => ct.CourseId == courseId).ToListAsync();

            if (Request.Query.Count == 1 && Request.Query.ContainsKey("teacherId"))
                return await query.Where(ct => ct.TeacherId == teacherId).ToListAsync();

            // If none of the above conditions/scenarios apply, return a BadRequest response
            return BadRequest();
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
