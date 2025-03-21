using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassModules
{
    public class SocialScholarships
    {
        public int ScholarshipID { get; set; }
        public int StudentID { get; set; }
        public string DocumentReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Files { get; set; }
    }
}
