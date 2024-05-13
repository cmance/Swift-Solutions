using PopNGo.Models.DTO;

namespace PopNGo.Models.APIResponse
{
    public class EmailHistoryResponse
    {
        public List<string> Labels { get; set; }
        public List<EmailHistoryDTO> Data { get; set; }
        public List<int> Buckets { get; set; }
    }
}