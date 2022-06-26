using HomeSensors.Base.Interfaces;
using HomeSensors.Base.Interfaces.Services;
using HomeSensors.Base.Models;
using System.Text;

namespace HomeSensors.Services
{
    internal class SensorLogProcessor : ISensorLogProcessor
    {
        private ISensorFactory _sensorFactory;
        private ISensor _currentSensor;

        private Dictionary<string, ISensor> _sensors = new Dictionary<string, ISensor>();

        public SensorLogProcessor(ISensorFactory sensorFactory)
        {
            _sensorFactory = sensorFactory;
        }

        private SensorReference _sensorReference;

        public void ProcessLog(string logContentsStr)
        {
            foreach (var line in logContentsStr.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                ProcessLine(line);
            }
        }

        public string GetSensorsQualityRating()
        {
            StringBuilder sb = new();

            foreach (var sensor in _sensors.Values)
            {
                sb.AppendLine(sensor.ToString());                
            }

            return sb.ToString();
        }

        private void ProcessLine(string line)
        {
            string[] values = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (values.Length <= 1)
            {
                return;
            }

            if (values[0] == "reference")
            {
                ProcessReference(values);
            }
            else if (DateTime.TryParse(values[0], out var dateTime))
            {
                ProcessSensorData(dateTime, values[1]);
            }
            else
            {
                ProcessSensor(values[0], values[1]);                
            }
        }

        private void ProcessSensor(string type, string name)
        {
            if (_sensors.ContainsKey(name))
            {
                _currentSensor = _sensors[name];
            }
            else
            {
                _currentSensor = _sensorFactory.CreateSensor(type, name, _sensorReference);
                _sensors.Add(name, _currentSensor);
            }            
        }

        // This needs to be modified everytime new reference is added. Format of log file should be changed.
        private void ProcessReference(string[] values)
        {
            if (_sensorReference != null)
            {
                throw new InvalidDataException("Log can not contain more than 1 reference line.");
            }

            if (values.Length != 4)
            {
                throw new InvalidDataException($"Log does not contain valid reference data. Reference data:{string.Join(" ", values)}.");
            }

            _sensorReference = new SensorReference
            {
                Temperature = double.Parse(values[1]),
                Humidity = double.Parse(values[2]),
                CarbonMonoxidePPM = int.Parse(values[3])
            };
        }

        private void ProcessSensorData(DateTime timestamp, string value)
        {
            if (_currentSensor == null)
            {
                throw new InvalidDataException("Log can not contain data before sensor definition.");
            }

            _currentSensor.AddData(timestamp, value);
        }
    }
}
