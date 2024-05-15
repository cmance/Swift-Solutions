using PopNGo.Services;

namespace PopNGo.Models.DTO
{
    public class PlaceSuggestion
    {
        public int Position { get; set; }
        public string Title { get; set; }
        public string Place_Id { get; set; }
        public string Data_Id { get; set; }
        public string Data_Cid { get; set; }
        public string Reviews_Link { get; set; }
        public string Photos_Link { get; set; }
        public GpsCoordinates GPS_Coordinates { get; set; }
        public string Place_Id_Search { get; set; }
        public string Provider_Id { get; set; }
        public double Rating { get; set; }
        public int Reviews { get; set; }
        public string Price { get; set; }
        public bool Unclaimed_Listing { get; set; }
        public string Type { get; set; }
        public List<string> Types { get; set; }
        public string Type_id { get; set; }
        public List<string> Type_ids { get; set; }
        public string Address { get; set; }
        public string Open_State { get; set; }
        public string Hours { get; set; }
        public OperatingHours Operating_Hours { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public ServiceOptions Service_Options { get; set; }
        public string Order_Online { get; set; }
        public string Thumbnail { get; set; }
        public string Reserve_a_Table { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class PlaceSuggestionExtensions
    {
        public static PopNGo.Models.DTO.PlaceSuggestion ToDTO(this PopNGo.Models.PlaceSuggestion placeSuggestion)
        {
            return new PopNGo.Models.DTO.PlaceSuggestion
            {
                Title = placeSuggestion.Title,
                Address = placeSuggestion.Address,
                Phone = placeSuggestion.Phone,
                Thumbnail = placeSuggestion.Thumbnail,
                Order_Online = placeSuggestion.Order_Online,
                Reserve_a_Table = placeSuggestion.Reserve_a_Table,
                Description = placeSuggestion.Description,
                Hours = placeSuggestion.Hours,
                Monday = placeSuggestion.Operating_Hours.monday.ToString(),
                Tuesday = placeSuggestion.Operating_Hours.tuesday.ToString(),
                Wednesday = placeSuggestion.Operating_Hours.wednesday.ToString(),
                Thursday = placeSuggestion.Operating_Hours.thursday.ToString(),
                Friday = placeSuggestion.Operating_Hours.friday.ToString(),
                Saturday = placeSuggestion.Operating_Hours.saturday.ToString(),
                Sunday = placeSuggestion.Operating_Hours.sunday.ToString()
            };
        }
    }
}