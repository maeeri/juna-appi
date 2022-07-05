using System;
using System.Collections.Generic;
using System.Text;


public class Rootobject
{
    public Class1[] Property1 { get; set; }
}

public class Class1
{
    public string departureDate { get; set; }
    public Journeysection[] journeySections { get; set; }
    public string operatorShortCode { get; set; }
    public int operatorUICCode { get; set; }
    public string trainCategory { get; set; }
    public int trainNumber { get; set; }
    public string trainType { get; set; }
    public long version { get; set; }
}

public class Journeysection
{
    public int attapId { get; set; }
    public Begintimetablerow beginTimeTableRow { get; set; }
    public Endtimetablerow endTimeTableRow { get; set; }
    public Locomotive[] locomotives { get; set; }
    public int maximumSpeed { get; set; }
    public int saapAttapId { get; set; }
    public int totalLength { get; set; }
    public Wagon[] wagons { get; set; }
}

public class Begintimetablerow
{
    public string countryCode { get; set; }
    public DateTime scheduledTime { get; set; }
    public string stationShortCode { get; set; }
    public int stationUICCode { get; set; }
    public string type { get; set; }
}

public class Endtimetablerow
{
    public string countryCode { get; set; }
    public DateTime scheduledTime { get; set; }
    public string stationShortCode { get; set; }
    public int stationUICCode { get; set; }
    public string type { get; set; }
}

public class Locomotive
{
    public int location { get; set; }
    public string locomotiveType { get; set; }
    public string powerType { get; set; }
    public string vehicleNumber { get; set; }
}

public class Wagon
{
    public bool catering { get; set; }
    public bool disabled { get; set; }
    public int length { get; set; }
    public int location { get; set; }
    public bool luggage { get; set; }
    public bool pet { get; set; }
    public bool playground { get; set; }
    public int salesNumber { get; set; }
    public bool smoking { get; set; }
    public string vehicleNumber { get; set; }
    public bool video { get; set; }
    public string wagonType { get; set; }
}
