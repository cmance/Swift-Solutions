using PopNGo.Services;

namespace PopNGo.Models
{
    public class PlaceSuggestion
    {
        public string Title { get; set; }
        public GpsCoordinates GPS_Coordinates { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Open_state { get; set; }
        public string Hours { get; set; }
        public OperatingHours Operating_Hours { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public ServiceOptions Service_Options { get; set; }
        public string Order_Online { get; set; }
        public string Thumbnail { get; set; }
        public string Reserve_a_Table { get; set; }
    }
}
