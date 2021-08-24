using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models
{
    public class CourseTeacher
    {
        public long CourseId { get; set; }

        public long TeacherId { get; set; }

        public Course AssignedCourse { get; set; }

        public Teacher AssignedTeacher { get; set; }
    }
}
