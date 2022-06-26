using HomeSensors.Base.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HomeSensors.Services
{
    public static class SensorEvaluator
    {
        public static string EvaluateLogFile(string logContentsStr)
        {
            var serviceProvider = new ServiceCollection()
               .AddSingleton<ISensorLogProcessor, SensorLogProcessor>()
               .AddSingleton<ISensorFactory, SensorFactory>()
               .BuildServiceProvider();

            var sensorLogProcessor = serviceProvider.GetRequiredService<ISensorLogProcessor>();

            sensorLogProcessor.ProcessLog(logContentsStr);
            return sensorLogProcessor.GetSensorsQualityRating();
        }
    }
}
