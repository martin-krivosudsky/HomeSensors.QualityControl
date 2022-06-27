using HomeSensors.Base.Interfaces;
using HomeSensors.Base.Interfaces.Services;
using HomeSensors.Base.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace HomeSensors.Services
{
    internal class SensorFactory : ISensorFactory
    {
        private readonly ILogger<SensorFactory> _logger;

        public SensorFactory(ILogger<SensorFactory> logger)
        {
            _logger = logger;
        }

        // Caching compiled expressions. Much faster than reflection and almost as fast as new()
        private readonly Dictionary<string, Func<string, SensorReference, ISensor>> _constructors = new Dictionary<string, Func<string, SensorReference, ISensor>>();

        public ISensor CreateSensor(string type, string name, SensorReference sensorReference)
        {
            if (_constructors.ContainsKey(type))
            {
                return _constructors[type](name, sensorReference);
            }

            var reflectionType = typeof(SensorFactory)
                .Assembly
                .GetTypes()
                .FirstOrDefault(t => !t.IsAbstract && t.Name.ToLower() == type.ToLower());

            var ctor = reflectionType?.GetConstructor(new Type[] { typeof(string), typeof(SensorReference) });

            if (ctor == null)
            {
                _logger.LogError($"Type '{type}' is not supported.");
                throw new InvalidDataException($"Reflection failed. Type '{type}' not implemented.");
            }

            var parameter1 = Expression.Parameter(typeof(string), "name");
            var parameter2 = Expression.Parameter(typeof(SensorReference), "sensorReference");
            _constructors[type] = Expression.Lambda<Func<string, SensorReference, ISensor>>(
                Expression.New(ctor, new Expression[] { parameter1, parameter2 }), parameter1, parameter2).Compile();

            return _constructors[type](name, sensorReference);
        }
    }
}
