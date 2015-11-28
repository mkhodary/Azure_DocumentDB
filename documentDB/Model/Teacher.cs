using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace documentDb.Model
{
    public class Teacher
    {
        public Guid TeacherId { get; set; }

        public string FullName { get; set; }

        public int Age { get; set; }
    }
}
