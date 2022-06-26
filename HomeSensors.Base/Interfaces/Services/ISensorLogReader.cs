namespace HomeSensors.Base.Interfaces.Services
{
    public interface ISensorLogReader
    {
        public List<ISensor> GetSensorData(StreamReader streamReader);
    }
}
