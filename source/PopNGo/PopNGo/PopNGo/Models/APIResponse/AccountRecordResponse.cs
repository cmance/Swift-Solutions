using PopNGo.Models.DTO;

namespace PopNGo.Models.APIResponse
{
    public class AccountRecordResponse
    {
        public List<string> Labels { get; set; }
        public List<AccountRecord> Data { get; set; }
        public List<Tuple<int, int>> Buckets { get; set; }
    }
}