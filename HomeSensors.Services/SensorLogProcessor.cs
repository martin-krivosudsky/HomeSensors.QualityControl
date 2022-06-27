using HomeSensors.Base.Interfaces;
using HomeSensors.Base.Interfaces.Services;
using HomeSensors.Base.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace HomeSensors.Services
{
    internal class SensorLogProcessor : ISensorLogProcessor
    {
        private readonly ISensorFactory _sensorFactory;
        private ISensor _currentSensor;
        private readonly ILogger<SensorLogProcessor> _logger;

        private Dictionary<string, ISensor> _sensors = new Dictionary<string, ISensor>();

        public SensorLogProcessor(ISensorFactory sensorFactory, ILogger<SensorLogProcessor> logger)
        {
            _sensorFactory = sensorFactory;
            _logger = logger;
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
            _logger.LogDebug($"Processing line: '{line}'.");

            string[] values = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (values.Length <= 1)
            {
                _logger.LogWarning($"Invalid line: '{line}'. Skipping.");

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
                _logger.LogDebug($"Creating new sensor. Type: '{type}'. Name: '{name}'.");

                _currentSensor = _sensorFactory.CreateSensor(type, name, _sensorReference);
                _sensors.Add(name, _currentSensor);
            }
        }

        private void ProcessReference(string[] values)
        {
            if (_sensorReference != null)
            {
                _logger.LogWarning($"Log can not contain more than 1 reference line. Skipping: '{string.Join(" ", values)}'.");
                return;
            }

            try
            {
                _logger.LogDebug($"Creating sensor reference: '{string.Join(" ", values)}'.");

                _sensorReference = new SensorReference(values.Skip(1).ToArray());
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Invalid definition of sensor reference: '{string.Join(" ", values)}'.");
                throw;
            }
        }

        private void ProcessSensorData(DateTime timestamp, string value)
        {
            if (_currentSensor == null)
            {
                _logger.LogWarning($"Log can not contain data before sensor definition. Skipping: '{timestamp} {value}'.");
                return;
            }

            _currentSensor.AddData(timestamp, value);
        }
    }
}
