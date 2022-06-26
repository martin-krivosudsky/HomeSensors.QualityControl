using HomeSensors.Base.Models;

namespace HomeSensors.Base.Interfaces.Services
{
    public interface ISensorFactory
    {
        public ISensor CreateSensor(string type, string name, SensorReference sensorReference);
    }
}
