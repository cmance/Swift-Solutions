using System.Text.Json;
using System.Text.Json.Serialization;
using Humanizer;
using PopNGo.Models;

namespace PopNGo.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherForecastService> _logger;

        public WeatherForecastService(HttpClient httpClient, ILogger<WeatherForecastService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Weather> GetForecast(string location)
        {
            string endpoint = $"forecast?location={location}&aggregateHours=24&shortColumnNames=0&unitGroup=us&contentType=json";

            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var results = JsonSerializer.Deserialize<WeatherResponse>(responseBody, options);

                if (results == null)
                {
                    _logger.LogError("Failed to deserialize weather response");
                    return null;
                }

                var forecasts = new List<WeatherForecast>();
                foreach (var value in results.Locations.Values.FirstOrDefault().Values)
                {
                    DateTime date = value.DatetimeStr.AtMidnight();
                    DateTime now = DateTime.Now.AtMidnight();
                    if (date < now || date > now.AddDays(6))
                    {
                        continue;
                    }

                    forecasts.Add(new WeatherForecast
                    {
                        Date = value.DatetimeStr,
                        Condition = value.Conditions,
                        MinTemp = value.Mint,
                        MaxTemp = value.Maxt,
                        CloudCover = value.Cloudcover,
                        PrecipitationType = value.Preciptype,
                        PrecipitationAmount = value.Precip,
                        PrecipitationChance = value.Pop,
                        Humidity = value.Humidity
                    });
                }

                return new Weather
                {
                    Latitude = (decimal)results.Locations.Values.FirstOrDefault()?.Latitude,
                    Longitude = (decimal)results.Locations.Values.FirstOrDefault()?.Longitude,
                    DateCached = DateTime.Now.AtMidnight(),
                    WeatherForecasts = forecasts
                };
            }
            else
            {
                _logger.LogError($"Failed to retrieve weather: {response.StatusCode}\n{response.Content}");
                return null;
            }
        }
    }

    public class LocationInfo
    {
        [JsonIgnore]
        [JsonPropertyName("stationContributions")]
        public object StationContributions { get; set; }

        [JsonPropertyName("values")]
        public List<Value> Values { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("time")]
        public double Time { get; set; }

        [JsonPropertyName("tz")]
        public string Tz { get; set; }

        [JsonPropertyName("currentConditions")]
        public CurrentConditions CurrentConditions { get; set; }

        [JsonIgnore]
        [JsonPropertyName("alerts")]
        public object Alerts { get; set; }
    }

    public class ColumnInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("unit")]
        public object Unit { get; set; }
    }

    public class Columns
    {
        [JsonPropertyName("wdir")]
        public ColumnInfo Wdir { get; set; }

        [JsonPropertyName("uvindex")]
        public ColumnInfo Uvindex { get; set; }

        [JsonPropertyName("latitude")]
        public ColumnInfo Latitude { get; set; }

        [JsonPropertyName("preciptype")]
        public ColumnInfo Preciptype { get; set; }

        [JsonPropertyName("cin")]
        public ColumnInfo Cin { get; set; }

        [JsonPropertyName("cloudcover")]
        public ColumnInfo Cloudcover { get; set; }

        [JsonPropertyName("pop")]
        public ColumnInfo Pop { get; set; }

        [JsonPropertyName("mint")]
        public ColumnInfo Mint { get; set; }

        [JsonPropertyName("datetime")]
        public ColumnInfo Datetime { get; set; }

        [JsonPropertyName("precip")]
        public ColumnInfo Precip { get; set; }

        [JsonPropertyName("solarradiation")]
        public ColumnInfo Solarradiation { get; set; }

        [JsonPropertyName("dew")]
        public ColumnInfo Dew { get; set; }

        [JsonPropertyName("humidity")]
        public ColumnInfo Humidity { get; set; }

        [JsonPropertyName("longitude")]
        public ColumnInfo Longitude { get; set; }

        [JsonPropertyName("temp")]
        public ColumnInfo Temp { get; set; }

        [JsonPropertyName("address")]
        public ColumnInfo Address { get; set; }

        [JsonPropertyName("maxt")]
        public ColumnInfo Maxt { get; set; }

        [JsonPropertyName("visibility")]
        public ColumnInfo Visibility { get; set; }

        [JsonPropertyName("wspd")]
        public ColumnInfo Wspd { get; set; }

        [JsonPropertyName("severerisk")]
        public ColumnInfo Severerisk { get; set; }

        [JsonPropertyName("solarenergy")]
        public ColumnInfo Solarenergy { get; set; }

        [JsonPropertyName("resolvedAddress")]
        public ColumnInfo ResolvedAddress { get; set; }

        [JsonPropertyName("heatindex")]
        public ColumnInfo Heatindex { get; set; }

        [JsonPropertyName("snowdepth")]
        public ColumnInfo Snowdepth { get; set; }

        [JsonPropertyName("sealevelpressure")]
        public ColumnInfo Sealevelpressure { get; set; }

        [JsonPropertyName("snow")]
        public ColumnInfo Snow { get; set; }

        [JsonPropertyName("name")]
        public ColumnInfo Name { get; set; }

        [JsonPropertyName("wgust")]
        public ColumnInfo Wgust { get; set; }

        [JsonPropertyName("conditions")]
        public ColumnInfo Conditions { get; set; }

        [JsonPropertyName("windchill")]
        public ColumnInfo Windchill { get; set; }

        [JsonPropertyName("cape")]
        public ColumnInfo Cape { get; set; }
    }

    public class CurrentConditions
    {
        [JsonPropertyName("wdir")]
        public double Wdir { get; set; }

        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("sunrise")]
        public DateTime Sunrise { get; set; }

        [JsonPropertyName("visibility")]
        public double Visibility { get; set; }

        [JsonPropertyName("wspd")]
        public double Wspd { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("stations")]
        public string Stations { get; set; }

        [JsonPropertyName("heatindex")]
        public object Heatindex { get; set; }

        [JsonPropertyName("cloudcover")]
        public double Cloudcover { get; set; }

        [JsonPropertyName("datetime")]
        public DateTime Datetime { get; set; }

        [JsonPropertyName("precip")]
        public double Precip { get; set; }

        [JsonPropertyName("moonphase")]
        public double Moonphase { get; set; }

        [JsonPropertyName("snowdepth")]
        public object Snowdepth { get; set; }

        [JsonPropertyName("sealevelpressure")]
        public double Sealevelpressure { get; set; }

        [JsonPropertyName("dew")]
        public double Dew { get; set; }

        [JsonPropertyName("sunset")]
        public DateTime Sunset { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("wgust")]
        public double Wgust { get; set; }

        [JsonPropertyName("windchill")]
        public object Windchill { get; set; }
    }

    // public class Locations
    // {
    //     public LocationInfo _448485123234 { get; set; }
    // }

    public class WeatherResponse
    {
        [JsonPropertyName("columns")]
        public Columns Columns { get; set; }

        [JsonPropertyName("remainingCost")]
        public int RemainingCost { get; set; }

        [JsonPropertyName("queryCost")]
        public int QueryCost { get; set; }

        [JsonIgnore]
        [JsonPropertyName("messages")]
        public object Messages { get; set; }

        [JsonPropertyName("locations")]
        // public LocationInfo Location { get; set; }
        public Dictionary<string, LocationInfo> Locations { get; set; }
        // public Locations Locations { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("wdir")]
        public double Wdir { get; set; }

        [JsonPropertyName("uvindex")]
        public double Uvindex { get; set; }

        [JsonPropertyName("datetimeStr")]
        public DateTime DatetimeStr { get; set; }

        [JsonPropertyName("preciptype")]
        public string Preciptype { get; set; }

        [JsonPropertyName("cin")]
        public double Cin { get; set; }

        [JsonPropertyName("cloudcover")]
        public double Cloudcover { get; set; }

        [JsonPropertyName("pop")]
        public double Pop { get; set; }

        [JsonPropertyName("mint")]
        public double Mint { get; set; }

        [JsonPropertyName("datetime")]
        public object Datetime { get; set; }

        [JsonPropertyName("precip")]
        public double Precip { get; set; }

        [JsonPropertyName("solarradiation")]
        public double Solarradiation { get; set; }

        [JsonPropertyName("dew")]
        public double Dew { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("maxt")]
        public double Maxt { get; set; }

        [JsonPropertyName("visibility")]
        public double Visibility { get; set; }

        [JsonPropertyName("wspd")]
        public double Wspd { get; set; }

        [JsonPropertyName("severerisk")]
        public double Severerisk { get; set; }

        [JsonPropertyName("solarenergy")]
        public double Solarenergy { get; set; }

        [JsonPropertyName("heatindex")]
        public object Heatindex { get; set; }

        [JsonPropertyName("snowdepth")]
        public double Snowdepth { get; set; }

        [JsonPropertyName("sealevelpressure")]
        public double Sealevelpressure { get; set; }

        [JsonPropertyName("snow")]
        public double Snow { get; set; }

        [JsonPropertyName("wgust")]
        public double Wgust { get; set; }

        [JsonPropertyName("conditions")]
        public string Conditions { get; set; }

        [JsonPropertyName("windchill")]
        public double? Windchill { get; set; }

        [JsonPropertyName("cape")]
        public double Cape { get; set; }
    }
} 