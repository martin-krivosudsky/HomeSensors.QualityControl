using HomeSensors.Base.Interfaces.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace HomeSensors.Services.Tests
{
    public class SensorLogProcessorTests
    {
        private readonly SensorLogProcessor _service;
        private readonly ISensorFactory _sensorFactory = Substitute.For<ISensorFactory>();
        private readonly MockLogger<SensorLogProcessor> _logger = Substitute.For<MockLogger<SensorLogProcessor>>();

        public SensorLogProcessorTests()
        {
            _service = new SensorLogProcessor(_sensorFactory, _logger);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ProcessLogTest(string input, LogLevel logLevel, string logMessage, Type exType)
        {
            if (exType != typeof(Ok))
            {
                try
                { 
                    _service.ProcessLog(input);
                }
                catch(Exception ex)
                {
                    Assert.Equal(exType, ex.GetType());
                }
            }
            else
            {
                _service.ProcessLog(input);
            }

            _logger.Received(1).Log(logLevel, Arg.Is<string>(m => m.Contains(logMessage)));
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                    new object[] { "invalidLine", LogLevel.Warning, "Invalid line", typeof(Ok) },
                    new object[] { "reference invalid", LogLevel.Error, "Invalid definition of sensor reference", typeof(FormatException) },
                    new object[] { "reference 1 1 1\r\nreference 2 2 2", LogLevel.Warning, "Log can not contain more than 1 reference line. Skipping", typeof(Ok) },
                    new object[] { $"{new DateTime()} 10", LogLevel.Warning, "Log can not contain data before sensor definition. Skipping:", typeof(Ok) },
                    new object[] { "someSensor name", LogLevel.Warning, "Log can not contain sensor definition before defining reference. Skipping", typeof(Ok) },
                    new object[] { "reference 1 1 1\r\nsomeSensor name", LogLevel.Debug, "Creating new sensor. Type: 'someSensor'. Name: 'name'.", typeof(Ok) },
                    new object[] { $"reference 1 1 1\r\nsomeSensor sensorName\r\n {new DateTime()} 10", LogLevel.Debug, $"Adding reading", typeof(Ok) }
            };
        private class Ok { };
    }
}