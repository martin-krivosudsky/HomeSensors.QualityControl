using HomeSensors.Base;
using HomeSensors.Base.Enums;
using HomeSensors.Base.Models;

namespace HomeSensors.Services.Models;

public class Thermometer : SensorBase<double>
{
    public Thermometer(string name, SensorReference sensorReference) : base (name, sensorReference.Temperature)
    { }

    public override QualityRating GetQualityRating()
    {
        var values = _data.Select(d => d.Value);

        double average = values.Average();
        double standardDeviation = Math.Sqrt(values.Average(v => Math.Pow(v - average, 2)));

        if (Math.Abs(average - _measuredValue) <= Constants.MaximumMeanTemperatureDifference)
        {
            if (standardDeviation <= Constants.MaximumUltraPreciseTemperatureStandardDeviation)
            {
                return QualityRating.UltraPrecise;
            }

            if (standardDeviation <= Constants.MaximumVeryPreciseTemperatureStandardDeviation)
            {
                return QualityRating.VeryPrecise;
            }
        }

        return QualityRating.Precise;
    }
}
