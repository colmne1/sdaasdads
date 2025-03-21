using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassModules
{
    public class Obshaga
    {
        public int DormitoryID { get; set; }
        public int StudentID { get; set; }
        public int RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Note { get; set; }
        public string Files { get; set; }
    }
}
