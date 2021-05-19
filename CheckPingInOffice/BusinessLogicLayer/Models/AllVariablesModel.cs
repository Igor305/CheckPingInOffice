using System.Collections.Generic;
using System.Timers;

namespace BusinessLogicLayer.Models
{
    public class AllVariablesModel
    {
        public string ipAddress { get; set; }

        public double nAllSendLastHour { get; set; }
        public double nTrueSendLastHour { get; set; }
        public double nFalseSendLastHour { get; set; }
        public double percentsLastHour { get; set; }  
        public List<bool> lastHour { get; set; }

        public double nAllSendForDay { get; set; }
        public double nTrueSendForDay { get; set; }
        public double nFalseSendForDay { get; set; }
        public double percentsForDay { get; set; }

        public double nAllSendYesterday { get; set; }
        public double nTrueSendYesterday { get; set; }
        public double nFalseSendYesterday { get; set; }
        public double percentsYesterday { get; set; }

        public Timer timer { get; set; }

        public AllVariablesModel()
        {
            lastHour = new List<bool>();
        }
    }
}
