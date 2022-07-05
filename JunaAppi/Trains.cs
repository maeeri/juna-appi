using System;

public class Junat
{
    public Juna[] juna { get; set; }
}

public class Juna
{
    public bool cancelled { get; set; }
    public string commuterLineID { get; set; }
    public bool deleted { get; set; }
    public string departureDate { get; set; }
    public string operatorShortCode { get; set; }
    public int operatorUICCode { get; set; }
    public bool runningCurrently { get; set; }
    public Timetablerow[] timeTableRows { get; set; }
    public DateTime timetableAcceptanceDate { get; set; }
    public string timetableType { get; set; }
    public string trainCategory { get; set; }
    public int trainNumber { get; set; }
    public string trainType { get; set; }
    public int version { get; set; }
}

public class Timetablerow
{
    public DateTime actualTime { get; set; }
    public bool cancelled { get; set; }
    public Caus[] causes { get; set; }
    public bool commercialStop { get; set; }
    public string commercialTrack { get; set; }
    public string countryCode { get; set; }
    public int differenceInMinutes { get; set; }
    public string estimateSource { get; set; }
    public DateTime liveEstimateTime { get; set; }
    public DateTime scheduledTime { get; set; }
    public string stationShortCode { get; set; }
    public int stationUICCode { get; set; }
    public Trainready trainReady { get; set; }
    public bool trainStopping { get; set; }
    public string type { get; set; }
    public bool unknownDelay { get; set; }
}

public class Trainready
{
    public bool accepted { get; set; }
    public string source { get; set; }
    public DateTime timestamp { get; set; }
}

public class Caus
{
    public string categoryCode { get; set; }
    public int categoryCodeId { get; set; }
    public string categoryName { get; set; }
    public string description { get; set; }
    public string detailedCategoryCode { get; set; }
    public int detailedCategoryCodeId { get; set; }
    public string detailedCategoryName { get; set; }
    public int id { get; set; }
    public Passengerterm passengerTerm { get; set; }
    public string thirdCategoryCode { get; set; }
    public int thirdCategoryCodeId { get; set; }
    public string thirdCategoryName { get; set; }
    public string validFrom { get; set; }
    public string validTo { get; set; }
}

public class Passengerterm
{
    public string en { get; set; }
    public string fi { get; set; }
    public string sv { get; set; }
}
