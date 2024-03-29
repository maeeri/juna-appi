﻿using System;

namespace JunaAppi
{
        public class Reitti
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
            public Timetablerow2[] timeTableRows { get; set; }
        }

        public class Timetablerow2
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

