using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IAccountRecordRepository : IRepository<AccountRecord>
    {
        public void AdjustBalance(DateTime day, string field, string direction);

        public Task<List<AccountRecord>> GetAccountRecordsWithinTime(int day);
    }
}
