using HomeSensors.Base.Enums;

namespace HomeSensors.Base.Interfaces;

public interface ISensor
{
    public QualityRating GetQualityRating();
    public void AddData(DateTime timestamp, string value);
}
