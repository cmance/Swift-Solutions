using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PopNGo.Models;

namespace PopNGo.Services
{
    public class Detail
    {
        public string title { get; set; }
        public string action { get; set; }
        public int distance { get; set; }
        public int duration { get; set; }
        public string formatted_distance { get; set; }
        public string formatted_duration { get; set; }
        public string geo_photo { get; set; }
        public GpsCoordinatesDirection gps_coordinates { get; set; }
        public List<string> extensions { get; set; }
    }

    public class Direction
    {
        public string travel_mode { get; set; }
        public string via { get; set; }
        public int distance { get; set; }
        public int duration { get; set; }
        public string typical_duration_range { get; set; }
        public string formatted_distance { get; set; }
        public string formatted_duration { get; set; }
        public List<string> extensions { get; set; }
        public List<Trip> trips { get; set; }
    }

    public class Duration
    {
        public string travel_mode { get; set; }
        public int duration { get; set; }
        public string formatted_duration { get; set; }
    }

    public class GpsCoordinatesDirection
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class PlacesInfo
    {
        public string address { get; set; }
        public string data_id { get; set; }
        public GpsCoordinatesDirection gps_coordinates { get; set; }
    }

    public class MapRoot
    {
        public SearchMetadata search_metadata { get; set; }
        public SearchParameters search_parameters { get; set; }
        public List<PlacesInfo> places_info { get; set; }
        public List<Direction> directions { get; set; }
        public List<Duration> durations { get; set; }
    }

    public class SearchMetadataDirection
    {
        public string id { get; set; }
        public string status { get; set; }
        public string json_endpoint { get; set; }
        public string created_at { get; set; }
        public string processed_at { get; set; }
        public string google_maps_directions_url { get; set; }
        public string raw_html_file { get; set; }
        public string prettify_html_file { get; set; }
        public double total_time_taken { get; set; }
    }

    public class SearchParametersDirection
    {
        public string engine { get; set; }
        public string hl { get; set; }
        public string start_addr { get; set; }
        public string end_addr { get; set; }
    }

    public class Trip
    {
        public string travel_mode { get; set; }
        public string title { get; set; }
        public int distance { get; set; }
        public int duration { get; set; }
        public string formatted_distance { get; set; }
        public string formatted_duration { get; set; }
        public List<Detail> details { get; set; }
    }

    public class MapDirectionsService : IMapDirectionsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MapDirectionsService> _logger;
        private readonly string _apiKey;

        public MapDirectionsService(HttpClient httpClient, ILogger<MapDirectionsService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<DirectionDetail>> GetDirections(string startAddress, string endAddress)
        {
            // Encode the start and end addresses and replace %20 with +
            var encodedStartAddress = Uri.EscapeDataString(startAddress).Replace("%20", "+");
            var encodedEndAddress = Uri.EscapeDataString(endAddress).Replace("%20", "+");

            // Construct the URL for the API call
            var url = $"https://serpapi.com/search.json?engine=google_maps_directions&start_addr={encodedStartAddress}&end_addr={encodedEndAddress}&api_key=a1bb7048235f3cbe3b6a28f598608d327812a86e2efffcb70bac91aefadd6e2f";
            //search.json?engine=google_maps_directions&start_addr=Austin-Bergstrom+International+Airport&end_addr=5540+N+Lamar+Blvd,+Austin,+TX+78756,+USA&travel_mode=2

            _logger.LogInformation("Request URL: {0}", url);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch data with status code: {0}, reason: {1}, content: {2}", response.StatusCode, response.ReasonPhrase, errorContent);
                    return null;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseBody))
                {
                    _logger.LogError("Received empty response body.");
                    return null;
                }

                try
                {
                    MapRoot root = JsonSerializer.Deserialize<MapRoot>(responseBody);
                    if (root?.directions == null) return null;
                    var googleMapsUrl = root.search_metadata.google_maps_url; // Extract the URL

                    return root.directions.Select(direction => new DirectionDetail
                    {
                        Travel_Mode = direction.travel_mode,
                        Via = direction.via,
                        Distance = direction.distance,
                        Duration = direction.duration,
                        Typical_Duration_Range = direction.typical_duration_range,
                        Formatted_Distance = direction.formatted_distance,
                        Formatted_Duration = direction.formatted_duration,
                        Extensions = direction.extensions,
                        Trips = direction.trips,
                        Google_Maps_Directions_Url = googleMapsUrl // Include this mapping

                    }).ToList();
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error parsing JSON from response: {0}", responseBody);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while making the API call.");
                return null;
            }
        }
    }
}