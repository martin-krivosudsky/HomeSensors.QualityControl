using HomeSensors.Base.Interfaces.DAL;
using System.Text;

namespace HomeSensors.DAL
{
    public class SensorLogDataFileDAL : ISensorLogDAL
    {
        private readonly string _filePath;

        public SensorLogDataFileDAL(string filePath)
        {
            _filePath = filePath;
        }

        public StreamReader GetStream()
        {
            using var fileStream = File.OpenRead(_filePath);
            return new StreamReader(fileStream, Encoding.UTF8, true);
        }
    }
}
