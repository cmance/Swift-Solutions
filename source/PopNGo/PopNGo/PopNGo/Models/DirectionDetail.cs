using PopNGo.Services;
using System.Collections.Generic;

namespace PopNGo.Models
{
    public class DirectionDetail
    {
        public string Travel_Mode { get; set; }
        public string Via { get; set; }
        public int Distance { get; set; }
        public int Duration { get; set; }
        public string Typical_Duration_Range { get; set; }
        public string Formatted_Distance { get; set; }
        public string Formatted_Duration { get; set; }
        public List<string> Extensions { get; set; }
        public List<Trip> Trips { get; set; }
        public string Google_Maps_Directions_Url { get; set; }

    }
}