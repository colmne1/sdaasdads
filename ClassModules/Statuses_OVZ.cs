using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassModules
{
     public class Statuses_OVZ
    {
        public int OVZStatusID { get; set; }
        public int StudentID { get; set; }
        public string Prikaz { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public string Files { get; set; }
    }
}
