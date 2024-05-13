using PopNGo.Models.DTO;

namespace PopNGo.Models.APIResponse
{
    public class SearchRecordResponse
    {
        public List<string> Labels { get; set; }
        public List<SearchRecordDTO> Data { get; set; }
        public List<int> Buckets { get; set; }
    }
}