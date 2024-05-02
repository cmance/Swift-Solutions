using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace PopNGo.DAL.Concrete
{
    public class SearchRecordRepository : Repository<SearchRecord>, ISearchRecordRepository
    {
        private readonly DbSet<SearchRecord> _searchRecords;
        public SearchRecordRepository(PopNGoDB context) : base(context)
        {
            _searchRecords = context.SearchRecords;
        }

        public async Task<List<SearchRecord>> GetSearchRecordsWithinTime(int day)
        {
            if(day < 0)
            {
                return await _searchRecords.ToListAsync();
            }

            DateTime check = day == 0 ? DateTime.Now.AtMidnight() : DateTime.Now.AtMidnight().AddDays(-day);
            var searchRecords = await _searchRecords.Where(sr => sr.Time >= check).ToListAsync();
            return searchRecords;
        }
    }
}