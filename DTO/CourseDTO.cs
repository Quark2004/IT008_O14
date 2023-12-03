using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
    internal class CourseDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public int numberofcredits { get; set; }
        public string schoolday { get; set; }
        public string lesson { get; set; }
        public string classroom { get; set; }
        public string semester { get; set; }
        public string schoolyear { get; set; }
        public DateTime startday { get; set; }
        public DateTime endday { get; set; }

    }
}
