using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace PopNGo.DAL.Concrete
{
    public class EmailHistoryRepository : Repository<EmailHistory>, IEmailHistoryRepository
    {
        private readonly DbSet<EmailHistory> _emailHistories;
        public EmailHistoryRepository(PopNGoDB context) : base(context)
        {
            _emailHistories = context.EmailHistories;
        }

        public async Task<List<EmailHistory>> GetEmailHistoriesWithinTime(int day)
        {
            if(day < 0)
            {
                return await _emailHistories.ToListAsync();
            }

            DateTime check = day == 0 ? DateTime.Now.AtMidnight() : DateTime.Now.AtMidnight().AddDays(-day);
            var emailHistories = await _emailHistories.Where(eh => eh.TimeSent >= check).ToListAsync();
            return emailHistories;
        }
    }
}