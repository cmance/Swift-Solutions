using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IEmailHistoryRepository : IRepository<EmailHistory>
    {
        public Task<List<EmailHistory>> GetEmailHistoriesWithinTime(int day);
    }
}
