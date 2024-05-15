using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PopNGo.Models;

namespace PopNGo.Services
{
    public class GpsCoordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class AllLocalResult
    {
        public int position { get; set; }
        public string title { get; set; }
        public string place_id { get; set; }
        public string data_id { get; set; }
        public string data_cid { get; set; }
        public string reviews_link { get; set; }
        public string photos_link { get; set; }
        public GpsCoordinates gps_coordinates { get; set; }
        public string place_id_search { get; set; }
        public string provider_id { get; set; }
        public double rating { get; set; }
        public int reviews { get; set; }
        public string price { get; set; }
        public bool unclaimed_listing { get; set; }
        public string type { get; set; }
        public List<string> types { get; set; }
        public string type_id { get; set; }
        public List<string> type_ids { get; set; }
        public string address { get; set; }
        public string open_state { get; set; }
        public string hours { get; set; }
        public OperatingHours operating_hours { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public ServiceOptions service_options { get; set; }
        public string order_online { get; set; }
        public string thumbnail { get; set; }
        public string reserve_a_table { get; set; }
    }

    public class OperatingHours
    {
        public string wednesday { get; set; }
        public string thursday { get; set; }
        public string friday { get; set; }
        public string saturday { get; set; }
        public string sunday { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
    }

    public class Root
    {
        public SearchMetadata search_metadata { get; set; }
        public SearchParameters search_parameters { get; set; }
        public SearchInformation search_information { get; set; }
        public List<AllLocalResult> local_results { get; set; }
        public SerpapiPagination serpapi_pagination { get; set; }
    }

    public class SearchInformation
    {
        public string local_results_state { get; set; }
        public string query_displayed { get; set; }
    }

    public class SearchMetadata
    {
        public string id { get; set; }
        public string status { get; set; }
        public string json_endpoint { get; set; }
        public string created_at { get; set; }
        public string processed_at { get; set; }
        public string google_maps_url { get; set; }
        public string raw_html_file { get; set; }
        public double total_time_taken { get; set; }
    }

    public class SearchParameters
    {
        public string engine { get; set; }
        public string type { get; set; }
        public string q { get; set; }
        public string ll { get; set; }
        public string google_domain { get; set; }
        public string hl { get; set; }
    }

    public class SerpapiPagination
    {
        public string next { get; set; }
    }

    public class ServiceOptions
    {
        public bool dine_in { get; set; }
        public bool takeout { get; set; }
        public bool delivery { get; set; }
        public bool? curbside_pickup { get; set; }
        public bool? no_contact_delivery { get; set; }
    }

    public class PlaceSuggestionsService : IPlaceSuggestionsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PlaceSuggestionsService> _logger;

        public PlaceSuggestionsService(HttpClient httpClient, ILogger<PlaceSuggestionsService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<PlaceSuggestion>> SearchPlaceSuggestion(string query, string coordinates)
        {
            var encodedQuery = Uri.EscapeDataString(query);
            var url = $"{_httpClient.BaseAddress}engine=google_maps&q={encodedQuery}&ll={coordinates}&type=search&api_key={_httpClient.DefaultRequestHeaders.GetValues("X-RapidAPI-Key").FirstOrDefault()}";

            _logger.LogInformation("Request URL: {0}", url);  // Log the URL to verify it

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to fetch data with status code: {0}, reason: {1}, content: {2}",
                                 response.StatusCode, response.ReasonPhrase, errorContent);
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
                Root root = JsonSerializer.Deserialize<Root>(responseBody);
                if (root?.local_results == null) return null;

                return root.local_results.Select(result => new PlaceSuggestion
                {
                    Title = result.title,
                    GPS_Coordinates = new GpsCoordinates { latitude = result.gps_coordinates.latitude, longitude = result.gps_coordinates.longitude },
                    Type = result.type,
                    Address = result.address,
                    Hours = result.hours,
                    Open_state = result.open_state,
                    Operating_Hours = result.operating_hours,
                    Phone = result.phone,
                    Website = result.website,
                    Description = result.description,
                    Service_Options = result.service_options,
                    Order_Online = result.order_online,
                    Thumbnail = result.thumbnail,
                    Reserve_a_Table = result.reserve_a_table
                }).Take(5).ToList();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing JSON from response: {0}", responseBody);
                return null;
            }
        }
    }
}
