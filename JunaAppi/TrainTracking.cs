using System;

public class TrainTracking
{
    public TrainTrackingLatest[] Property1 { get; set; }
}

public class TrainTrackingLatest
{
    public string departureDate { get; set; }
    public int id { get; set; }
    public string nextStation { get; set; }
    public string nextTrackSection { get; set; }
    public string previousStation { get; set; }
    public string previousTrackSection { get; set; }
    public string station { get; set; }
    public DateTime timestamp { get; set; }
    public string trackSection { get; set; }
    public string trainNumber { get; set; }
    public string type { get; set; }
    public int version { get; set; }

    
}
