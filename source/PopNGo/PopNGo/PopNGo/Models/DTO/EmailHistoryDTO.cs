using Microsoft.Identity.Client;

namespace PopNGo.Models.DTO
{
    public class EmailHistoryDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TimeSent { get; set; }
        public string Type { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class EmailHistoryExtensions
    {
        public static Models.DTO.EmailHistoryDTO ToDTO(this Models.EmailHistory EmailHistory)
        {
            return new Models.DTO.EmailHistoryDTO
            {
                Id = EmailHistory.Id,
                UserId = EmailHistory.UserId,
                TimeSent = EmailHistory.TimeSent,
                Type = EmailHistory.Type
            };
        }
    }
}