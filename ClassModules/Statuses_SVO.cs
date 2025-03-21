using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassModules
{
    public class Statuses_SVO
    {
        public int SVOStatusID { get; set; }
        public int StudentID { get; set; }
        public string DocumentOsnov { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Files { get; set; }
    }
}
