using HomeSensors.Base.Enums;
using System.Text.RegularExpressions;

namespace HomeSensors.Base.Helpers
{
    internal static class EnumToString
    {
        public static string ToFriendlyString(this QualityRating qualityRating)
        {
            return string.Join(" ", Regex.Split(qualityRating.ToString(), @"(?<!^)(?=[A-Z])")).ToLower();
        }
    }
}
