using HomeSensors.Base.Enums;
using HomeSensors.Base.Helpers;
using HomeSensors.Base.Interfaces;

namespace HomeSensors.Base.Models;

public abstract class SensorBase<T> : ISensor where T : struct
{
    public SensorBase(string name, T measuredValue)
    {
        _name = name;
        _measuredValue = measuredValue;
        _data = new List<SensorReading<T>>();
    }

    private string _name { get; set; }
    protected List<SensorReading<T>> _data { get; set; }
    protected T _measuredValue { get; set; }

    public void AddData(DateTime timestamp, string value)
    {
        T convertedValue;

        try
        {
            convertedValue = (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return;
        }

        _data.Add(new SensorReading<T>()
        {
            TimeStamp = timestamp,
            Value = convertedValue
        });
    }

    public override string ToString() 
    {
        return $"\"{_name}\": \"{GetQualityRating().ToFriendlyString()}\"";
    }

    public abstract QualityRating GetQualityRating();
}
