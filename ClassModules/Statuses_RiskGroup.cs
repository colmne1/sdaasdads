using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassModules
{
    public class Statuses_RiskGroup
    {
        public int RiskGroupID { get; set; }
        public int StudentID { get; set; }
        public string RiskGroupType { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string OsnPost { get; set; }
        public string OsnSnat { get; set; }
        public string PrichinaPost { get; set; }
        public string PrichinaSnat { get; set; }
        public string Note { get; set; }
        public string Files { get; set; }
    }
}
