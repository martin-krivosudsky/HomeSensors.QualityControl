using HomeSensors.Base;
using HomeSensors.Base.Enums;
using HomeSensors.Base.Models;

namespace HomeSensors.Services.Models;

public class Humidity : SensorBase<double>
{
    public Humidity(string name, SensorReference sensorReference) : base (name, sensorReference.Humidity)
    { }

    public override QualityRating GetQualityRating()
    {
        var values = _data.Select(d => d.Value);

        if (_data.Any(v => Math.Abs(v.Value - _measuredValue) > Constants.MaximumHumidityDifference))
        {
            return QualityRating.Discard;
        }

        return QualityRating.Keep;
    }
}
