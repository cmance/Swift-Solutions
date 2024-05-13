using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface ISearchRecordRepository : IRepository<SearchRecord>
    {
        public Task<List<SearchRecord>> GetSearchRecordsWithinTime(int day);
    }
}
