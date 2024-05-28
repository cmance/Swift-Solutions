using PopNGo.Services;
using System.Collections.Generic;

namespace PopNGo.Models.DTO
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

namespace PopNGo.ExtensionMethods
{
    public static class DirectionDetailExtensions
    {
        public static PopNGo.Models.DTO.DirectionDetail ToDTO(this PopNGo.Models.DirectionDetail directionDetail)
        {
            return new PopNGo.Models.DTO.DirectionDetail
            {
                Travel_Mode = directionDetail.Travel_Mode,
                Via = directionDetail.Via,
                Distance = directionDetail.Distance,
                Duration = directionDetail.Duration,
                Typical_Duration_Range = directionDetail.Typical_Duration_Range,
                Formatted_Distance = directionDetail.Formatted_Distance,
                Formatted_Duration = directionDetail.Formatted_Duration,
                Extensions = directionDetail.Extensions,
                Trips = directionDetail.Trips,
                Google_Maps_Directions_Url = directionDetail.Google_Maps_Directions_Url
            };
        }
    }
}