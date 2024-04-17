﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PopNGo.Models;
using System.Linq.Expressions;
namespace PopNGo.Services
{
    class Parameter
    {
        public string query { get; set; }
        public int start { get; set; }
    }

    public class TicketLink
    {
        public string? source { get; set; }
        public string? link { get; set; }
    }

    class InfoLink
    {
        public string source { get; set; }
        public string link { get; set; }
    }

    public class Venue
    {
        public string google_id { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string website { get; set; }
        public int review_count { get; set; }
        public decimal rating { get; set; }
        public string subtype { get; set; }
        public List<string> subtypes { get; set; }
        public string full_address { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public string street_number { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string timezone { get; set; }
        public string google_mid { get; set; }
    }

    class Data
    {
        public string event_id { get; set; }
        public string event_mid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public bool is_virtual { get; set; }
        public string thumbnail { get; set; }
        public List<TicketLink> ticket_links { get; set; }
        public List<InfoLink> info_links { get; set; }
        public Venue venue { get; set; }
        public List<string> tags { get; set; }
        public string language { get; set; }
        public string link { get; set; } // Add this property
    }


    class AllEventDetail
    {
        public string status { get; set; }
        public string request_id { get; set; }
        public Parameter parameters { get; set; }
        public List<Data> data { get; set; }
    }

    class EventInfoDetail
    {
        public string status { get; set; }
        public string request_id { get; set; }
        public Parameter parameters { get; set; }
        public Data data { get; set; }
    }



    public class RealTimeEventSearchService : IRealTimeEventSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RealTimeEventSearchService> _logger;

        public RealTimeEventSearchService(HttpClient httpClient, ILogger<RealTimeEventSearchService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<EventDetail>> SearchEventAsync(string query, int start)
        {
            string endpoint = $"search-events?query={query}&start={start}";
            _logger.LogInformation($"Calling Real Time Search Event API at {endpoint}");

            HttpResponseMessage response = null;
            string responseBody = null;
            int retryCount = 0;

            while (retryCount < 5)
            {
                try
                {
                    response = await _httpClient.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation($"Response body: {responseBody}");
                        if (!string.IsNullOrEmpty(responseBody))
                        {
                            break;
                        }
                    }
                    else
                    {
                        HandleErrorResponse(response.StatusCode);
                        return Enumerable.Empty<EventDetail>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while searching for events");
                    return Enumerable.Empty<EventDetail>();
                }

                // Wait for 1 second before retrying
                await Task.Delay(1000);

                retryCount++;
            }

            if (string.IsNullOrEmpty(responseBody))
            {
                _logger.LogError("responseBody is still null or empty after 5 retries");
                return Enumerable.Empty<EventDetail>();
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            AllEventDetail allEventDetail = JsonSerializer.Deserialize<AllEventDetail>(responseBody, options);

            IList<EventDetail> eventDetails = allEventDetail.data.Select(data => new EventDetail
            {
                EventID = data.event_id,
                EventName = data.name,
                EventLink = data.link,
                EventDescription = data.description,
                EventStartTime = DateTime.TryParse(data.start_time, out DateTime startTime) ? startTime : DateTime.MinValue,
                EventEndTime = DateTime.TryParse(data.end_time, out DateTime endTime) ? endTime : DateTime.MinValue,
                EventIsVirtual = data.is_virtual,
                EventThumbnail = data.thumbnail,
                EventLanguage = data.language,
                Full_Address = data.venue?.full_address ?? "",
                Longitude = data.venue?.longitude ?? 0,
                Latitude = data.venue?.latitude ?? 0,
                Phone_Number = data.venue?.phone_number,
                VenueName = data.venue?.name,
                VenueRating = data.venue?.rating,
                VenueWebsite = data.venue?.website,
                TicketLinks = data.ticket_links != null ? data.ticket_links.Select(t => new PopNGo.Models.DTO.TicketLink
                {
                    Source = t.source,
                    Link = t.link
                }).ToList() : new List<PopNGo.Models.DTO.TicketLink>(),
                EventTags = data.tags
            }).ToList();

            return eventDetails;
        }
        public async Task<EventDetail> SearchEventInfoAsync(string eventId)
        {
            string endpoint = $"event-details?event_id={eventId}";

            _logger.LogInformation($"Calling Real Time Search Event API at {endpoint}");

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                string responseBody;

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Response body: {responseBody}");

                }
                else
                {
                    HandleErrorResponse(response.StatusCode);
                    responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error response body: {responseBody}");
                    return null;
                }

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Console.WriteLine($"Query: {endpoint},\nResponse: {responseBody}");
                EventInfoDetail newEvent = JsonSerializer.Deserialize<EventInfoDetail>(responseBody, options);

                if (newEvent == null)
                {
                    _logger.LogError("Deserialized event is null");
                    return null;
                }

                return new EventDetail
                {
                    EventID = newEvent.data.event_id,
                    EventName = newEvent.data.name,
                    EventLink = newEvent.data.link,
                    EventDescription = newEvent.data.description,
                    EventStartTime = DateTime.TryParse(newEvent.data.start_time, out DateTime startTime) ? startTime : DateTime.MinValue,
                    EventEndTime = DateTime.TryParse(newEvent.data.end_time, out DateTime endTime) ? endTime : DateTime.MinValue,
                    EventIsVirtual = newEvent.data.is_virtual,
                    EventThumbnail = newEvent.data.thumbnail,
                    EventLanguage = newEvent.data.language,
                    Full_Address = newEvent.data.venue?.full_address ?? "",
                    Longitude = newEvent.data.venue?.longitude ?? 0,
                    Latitude = newEvent.data.venue?.latitude ?? 0,
                    Phone_Number = newEvent.data.venue?.phone_number,
                    EventTags = newEvent.data.tags ?? new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return null;
            }
        }

        private void HandleErrorResponse(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError("Authentication failed. Check your API credentials.");
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError("Network failure. Check your API URL.");
            }
            else if (statusCode == HttpStatusCode.TooManyRequests)
            {
                _logger.LogError("Rate limit exceeded. Try again later.");
            }
            else
            {
                _logger.LogError($"Error: {statusCode}");
            }
        }
    }
}
