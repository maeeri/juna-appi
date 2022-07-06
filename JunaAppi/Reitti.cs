using System;
using System.Collections.Generic;
using System.Text;
using APIHelpers;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace JunaAppi
{
     public class Reitti
    {
        public class Reitinhaku
        {
            public Class1[] Property1 { get; set; }
        }

        public class Class1
        {
            public int trainNumber { get; set; }
            public string departureDate { get; set; }
            public int operatorUICCode { get; set; }
            public string operatorShortCode { get; set; }
            public string trainType { get; set; }
            public string trainCategory { get; set; }
            public string commuterLineID { get; set; }
            public bool runningCurrently { get; set; }
            public bool cancelled { get; set; }
            public long version { get; set; }
            public string timetableType { get; set; }
            public DateTime timetableAcceptanceDate { get; set; }
            public Timetablerow[] timeTableRows { get; set; }
        }

        public class Timetablerow
        {
            public string stationShortCode { get; set; }
            public int stationUICCode { get; set; }
            public string countryCode { get; set; }
            public string type { get; set; }
            public bool trainStopping { get; set; }
            public bool commercialStop { get; set; }
            public string commercialTrack { get; set; }
            public bool cancelled { get; set; }
            public DateTime scheduledTime { get; set; }
        }

    }
}
