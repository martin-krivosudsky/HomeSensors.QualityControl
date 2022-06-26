using HomeSensors.Services;

string path = "log.txt";

if (!File.Exists(path))
{
    return;
}

string log = File.ReadAllText(path);

string output = SensorEvaluator.EvaluateLogFile(log);

Console.WriteLine(output);