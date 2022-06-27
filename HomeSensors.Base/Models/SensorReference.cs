namespace HomeSensors.Base.Models
{
    public class SensorReference
    {
        // This needs to be modified everytime new reference is added. Format of log file should be changed (JSON maybe).
        public SensorReference(string[] input)
        {
            Temperature = double.Parse(input[0]);
            Humidity = double.Parse(input[1]);
            CarbonMonoxidePPM = int.Parse(input[2]);
        }

        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int CarbonMonoxidePPM { get; set; }
    }
}
