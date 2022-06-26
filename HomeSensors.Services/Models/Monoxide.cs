using HomeSensors.Base;
using HomeSensors.Base.Enums;
using HomeSensors.Base.Models;

namespace HomeSensors.Services.Models;

public class Monoxide : SensorBase<int>
{
    public Monoxide(string name, SensorReference sensorReference) : base (name, sensorReference.CarbonMonoxidePPM)
    { }

    public override QualityRating GetQualityRating()
    {
        if (_data.Any(v => Math.Abs(v.Value - _measuredValue) > Constants.MaximumPPMDifference))
        {
            return QualityRating.Discard;
        }

        return QualityRating.Keep;
    }
}
