using Microsoft.Identity.Client;

namespace PopNGo.Models.DTO
{
    public class WeatherForecastDTO
    {
        public DateTime Date { get; set; }
        public string Condition { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double CloudCover { get; set; }
        public string PrecipitationType { get; set; }
        public double PrecipitationAmount { get; set; }
        public double PrecipitationChance { get; set; }
        public double Humidity { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class WeatherForecastExtensions
    {
        public static Models.DTO.WeatherForecastDTO ToDTO(this Models.WeatherForecast Weather, string temperatureUnit, string measurementUnit)
        {
            return new Models.DTO.WeatherForecastDTO
            {
                Date = Weather.Date,
                Condition = Weather.Condition,
                MinTemp = ConvertTemperature(Weather.MinTemp, "f", temperatureUnit),
                MaxTemp = ConvertTemperature(Weather.MaxTemp, "f", temperatureUnit),
                CloudCover = Weather.CloudCover,
                PrecipitationType = Weather.PrecipitationType,
                PrecipitationAmount = ConvertMeasurement(Weather.PrecipitationAmount, "inches", measurementUnit),
                PrecipitationChance = Weather.PrecipitationChance,
                Humidity = Weather.Humidity
            };
        }

        private static double ConvertTemperature(double temperature, string fromUnit, string toUnit)
        {
            if (fromUnit == toUnit)
            {
                return temperature;
            }

            if (fromUnit == "f")
            {
                return (temperature - 32) * 5 / 9;
            }

            return temperature * 9 / 5 + 32;
        }

        private static double ConvertMeasurement(double measurement, string fromUnit, string toUnit)
        {
            if (fromUnit == toUnit)
            {
                return measurement;
            }

            if (fromUnit == "inches")
            {
                return measurement * 25.4;
            }

            return measurement / 25.4;
        }
    }
}