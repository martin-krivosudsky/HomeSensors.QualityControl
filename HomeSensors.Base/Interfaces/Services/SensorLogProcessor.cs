namespace HomeSensors.Base.Interfaces.Services
{
    public interface ISensorLogProcessor
    {
        public void ProcessLog(string logContentsStr);
        public string GetSensorsQualityRating();
    }
}
