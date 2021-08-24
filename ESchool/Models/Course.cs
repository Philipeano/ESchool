using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models
{
    public class Course
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public int Units { get; set; }
    }
}
