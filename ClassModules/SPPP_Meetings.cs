using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassModules
{
    public class SPPP_Meetings
    {
        public int MeetingID { get; set; }
        public int StudentID { get; set; }
        public DateTime Date { get; set; }
        public string OsnVizov { get; set; }
        public string Sotrudniki { get; set; }
        public string Predstaviteli { get; set; }
        public string ReasonForCall { get; set; }
        public string Reshenie { get; set; }
        public string Note { get; set; }
        public string Files { get; set; }
    }
}
