using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Services;
using System;
using System.Collections.Generic;

using System;
using PopNGo.Models.DTO;

namespace PopNGo.Models
{
    public class EventDetail
    {
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string EventLink { get; set; }
        public string EventDescription { get; set; }
        public DateTime? EventStartTime { get; set; }
        public DateTime? EventEndTime { get; set; }
        public bool EventIsVirtual { get; set; }
        public string EventThumbnail { get; set; }
        public string EventLanguage { get; set; }
        public string Full_Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Phone_Number { get; set; }
        public List<string> EventTags { get; set; } = [];
        public string VenueName { get; set; }
        public decimal? VenueRating { get; set; }
        public string VenueWebsite { get; set; }
        public IEnumerable<PopNGo.Models.DTO.TicketLink> TicketLinks { get; set; }
    }
}

