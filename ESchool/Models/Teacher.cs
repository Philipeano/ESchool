using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESchool.Models
{
    /// <summary>
    /// Defines a Teacher entity/model with properties belonging to any teacher.
    /// </summary>
    public class Teacher
    {    
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
