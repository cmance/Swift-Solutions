using Microsoft.Identity.Client;

namespace PopNGo.Models.DTO
{
    public class SearchRecordDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
        public string SearchQuery { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class SearchRecordExtensions
    {
        public static Models.DTO.SearchRecordDTO ToDTO(this Models.SearchRecord SearchRecord)
        {
            return new Models.DTO.SearchRecordDTO
            {
                Id = SearchRecord.Id,
                UserId = SearchRecord.UserId,
                Time = SearchRecord.Time,
                SearchQuery = SearchRecord.SearchQuery
            };
        }
    }
}