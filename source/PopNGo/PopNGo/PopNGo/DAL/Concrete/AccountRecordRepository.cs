using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;
using PopNGo.ExtensionMethods;
using Humanizer;

namespace PopNGo.DAL.Concrete
{
    public class AccountRecordRepository : Repository<AccountRecord>, IAccountRecordRepository
    {
        private readonly DbSet<AccountRecord> _accountRecords;
        private readonly PopNGoDB _context;

        public AccountRecordRepository(PopNGoDB context) : base(context)
        {
            _accountRecords = context.AccountRecords;
            _context = context;
        }

        public void AdjustBalance(DateTime day, string field, string direction)
        {
            var accountRecord = _accountRecords.FirstOrDefault(ar => ar.Day == day);
            if (accountRecord == null)
            {
                accountRecord = new AccountRecord { Day = day, AccountsCreated = 0, AccountsDeleted = 0 };

            }
            switch (field)
            {
                case "created":
                    accountRecord.AccountsCreated += direction == "increase" ? 1 : -1;
                    break;
                case "deleted":
                    accountRecord.AccountsDeleted += direction == "increase" ? 1 : -1;
                    break;
            }

            try
            {
                AddOrUpdate(accountRecord);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception, rethrow it, or handle it in some other way
                throw new Exception("Error adjusting account record balance", ex);
            }
        }

        public async Task<List<AccountRecord>> GetAccountRecordsWithinTime(int day)
        {
            if (day < 0)
            {
                return await GetAll().ToListAsync();
            }

            DateTime check = day == 0 ? DateTime.Now.AtMidnight() : DateTime.Now.AtMidnight().AddDays(-day);
            var accountRecords = await _accountRecords.Where(ar => ar.Day >= check ).ToListAsync();
            return accountRecords;
        }
    }
}
