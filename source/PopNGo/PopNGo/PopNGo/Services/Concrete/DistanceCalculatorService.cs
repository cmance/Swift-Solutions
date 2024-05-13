using System.Text.Json;
using System.Text.Json.Serialization;
using PopNGo.Models;

namespace PopNGo.Services
{
    public class EndPoint1
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint10
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint2
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint3
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint4
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint5
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint6
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint7
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint8
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class EndPoint9
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }

    public class DistanceResponse
    {
        [JsonPropertyName("start_point")]
        public StartPoint StartPoint { get; set; }

        [JsonPropertyName("end_point_1")]
        public EndPoint1 EndPoint1 { get; set; }

        [JsonPropertyName("end_point_2")]
        public EndPoint2 EndPoint2 { get; set; }

        [JsonPropertyName("end_point_3")]
        public EndPoint3 EndPoint3 { get; set; }

        [JsonPropertyName("end_point_4")]
        public EndPoint4 EndPoint4 { get; set; }

        [JsonPropertyName("end_point_5")]
        public EndPoint5 EndPoint5 { get; set; }

        [JsonPropertyName("end_point_6")]
        public EndPoint6 EndPoint6 { get; set; }

        [JsonPropertyName("end_point_7")]
        public EndPoint7 EndPoint7 { get; set; }

        [JsonPropertyName("end_point_8")]
        public EndPoint8 EndPoint8 { get; set; }

        [JsonPropertyName("end_point_9")]
        public EndPoint9 EndPoint9 { get; set; }

        [JsonPropertyName("end_point_10")]
        public EndPoint10 EndPoint10 { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }
    }

    public class StartPoint
    {
        [JsonPropertyName("coordinate")]
        public string Coordinate { get; set; }
    }

    public class DistanceReturn {
        public string Unit { get; set; }
        public List<double> Distances { get; set; }
    }

    public class DistanceCalculatorService : IDistanceCalculatorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DistanceCalculatorService> _logger;

        public DistanceCalculatorService(HttpClient httpClient, ILogger<DistanceCalculatorService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<DistanceReturn> CalculateDistance(string location, List<string> events, string units)
        {
            string endpoint = $"one_to_many?start_point={location}";
            int i = 0;
            foreach (var eventDetail in events)
            {
                i++;
                if (i <= 10 && eventDetail != null) { 
                    endpoint += $"&end_point_{i}={eventDetail}";
                }
            }
            endpoint += $"&unit={units}&decimal_places=2";

            Console.WriteLine(endpoint);
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var results = JsonSerializer.Deserialize<DistanceResponse>(responseBody, options);

                var distances = new List<double>();
                if(results.EndPoint1 != null)
                {
                    distances.Add(results.EndPoint1.Distance);
                }
                if(results.EndPoint2 != null)
                {
                    distances.Add(results.EndPoint2.Distance);
                }
                if(results.EndPoint3 != null)
                {
                    distances.Add(results.EndPoint3.Distance);
                }
                if(results.EndPoint4 != null)
                {
                    distances.Add(results.EndPoint4.Distance);
                }
                if(results.EndPoint5 != null)
                {
                    distances.Add(results.EndPoint5.Distance);
                }
                if(results.EndPoint6 != null)
                {
                    distances.Add(results.EndPoint6.Distance);
                }
                if(results.EndPoint7 != null)
                {
                    distances.Add(results.EndPoint7.Distance);
                }
                if(results.EndPoint8 != null)
                {
                    distances.Add(results.EndPoint8.Distance);
                }
                if(results.EndPoint9 != null)
                {
                    distances.Add(results.EndPoint9.Distance);
                }
                if(results.EndPoint10 != null)
                {
                    distances.Add(results.EndPoint10.Distance);
                }

                return new DistanceReturn
                {
                    Unit = results.Unit.ToLower() == "miles" ? "mi" : "km",
                    Distances = distances
                };
            }
            else
            {
                _logger.LogError($"Failed to calculate distance: {response.StatusCode}\n{response.Content}");
                return null;
            }
        }
    }
}