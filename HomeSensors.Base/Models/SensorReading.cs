namespace HomeSensors.Base.Models;

public class SensorReading<T> where T : struct
{
    public DateTime TimeStamp { get; set; }
    public T Value { get; set; }
}
