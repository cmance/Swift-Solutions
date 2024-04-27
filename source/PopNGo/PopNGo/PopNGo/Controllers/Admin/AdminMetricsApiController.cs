using Microsoft.AspNetCore.Mvc;
using PopNGo.DAL.Abstract;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
using PopNGo.ExtensionMethods;

using PopNGo.Models;
using Humanizer;
using PopNGo.Models.APIResponse;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/admin/metrics")]
public class AdminMetricsApiController : Controller
{
    private readonly ILogger<EventApiController> _logger;
    private readonly IEmailHistoryRepository _emailHistoryRepository;
    private readonly IAccountRecordRepository _accountRecordRepository;
    private readonly ISearchRecordRepository _searchRecordRepository;
    private readonly UserManager<PopNGoUser> _userManager;

    public AdminMetricsApiController(
        IEmailHistoryRepository emailHistoryRepository,
        IAccountRecordRepository accountRecordRepository,
        ISearchRecordRepository searchRecordRepository,
        ILogger<EventApiController> logger,
        UserManager<PopNGoUser> userManager
    )
    {
        _logger = logger;
        _emailHistoryRepository = emailHistoryRepository;
        _accountRecordRepository = accountRecordRepository;
        _searchRecordRepository = searchRecordRepository;
        _userManager = userManager;
    }

    [HttpGet("emails/time={time}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmailHistoryResponse))]
    public async Task<ActionResult<EmailHistoryResponse>> GetEmailMetrics(int time)
    {
        var emailHistoryList = buildLabels(time);
        var emailHistories = await _emailHistoryRepository.GetEmailHistoriesWithinTime(time);
        var emailHistoryDTOs = emailHistories.Select(e => e.ToDTO()).ToList();
        var emailHistoryBuckets = buildBuckets<EmailHistory>(
            time,
            emailHistoryList,
            emailHistories,
            e => e.TimeSent.DayOfYear,
            e => e.TimeSent.Hour,
            e => e.TimeSent.Date,
            e => e.TimeSent.Month,
            e => e.TimeSent.Year  
        );

        var emailHistoryResponse = new EmailHistoryResponse
        {
            Labels = emailHistoryList,
            Data = emailHistoryDTOs,
            Buckets = emailHistoryBuckets
        };
        return Ok(emailHistoryResponse);
    }

    [HttpGet("searches/time={time}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchRecordResponse))]
    public async Task<ActionResult<SearchRecordResponse>> GetSearchMetrics(int time)
    {
        var searchRecordList = buildLabels(time);
        var searchRecords = await _searchRecordRepository.GetSearchRecordsWithinTime(time);
        var searchRecordDTOs = searchRecords.Select(s => s.ToDTO()).ToList();
        var searchRecordBuckets = buildBuckets<SearchRecord>(
            time,
            searchRecordList,
            searchRecords,
            e => e.Time.DayOfYear,
            e => e.Time.Hour,
            e => e.Time.Date,
            e => e.Time.Month,
            e => e.Time.Year  
        );

        var searchRecordResponse = new SearchRecordResponse
        {
            Labels = searchRecordList,
            Data = searchRecordDTOs,
            Buckets = searchRecordBuckets
        };

        return Ok(searchRecordResponse);
    }

    [HttpGet("accounts/time={time}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AccountRecordResponse>))]
    public async Task<ActionResult<AccountRecordResponse>> GetAccountMetrics(int time)
    {
        var accountRecordList = buildLabels(time);
        if(time == 0 || time == 1)
        {
            accountRecordList = new List<string>();
            if(time == 0)
            {
                accountRecordList.Add("Today");
            }
            else
            {
                accountRecordList.Add("Yesterday");
            }
        }
        var accountRecords = await _accountRecordRepository.GetAccountRecordsWithinTime(time);
        var searchRecordBuckets = buildBucketsForAccounts(
            time,
            accountRecordList,
            accountRecords,
            e => e.Day.DayOfYear,
            e => e.Day.Date,
            e => e.Day.Month,
            e => e.Day.Year  
        );

        var accountRecordResponse = new AccountRecordResponse
        {
            Labels = accountRecordList,
            Data = accountRecords,
            Buckets = searchRecordBuckets
        };

        return Ok(accountRecordResponse);
    }

    [HttpGet("accounts/total")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public ActionResult<int> GetTotalAccounts()
    {
        var accountRecords = _userManager.Users.Count();

        return Ok(accountRecords);
    }

//  ==================================================
//  Helper Functions
    public List<string> buildLabels(int time)
    {
        var searchRecordList = new List<string>();
        List<string> months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        if(time < 2 && time >= 0)
        {
            for(int i = 0; i < 23; i++)
            {
                searchRecordList.Add(DateTime.Now.AtMidnight().AddHours(i).ToShortTimeString());
            }
        }
        else if(time == 7)
        {
            for(int i = 0; i < time; i++)
            {
                searchRecordList.Add(DateTime.Now.AddDays(-6 + i).Date.ToShortDateString());
            }
        }
        else if(time == 30)
        {
            for(int i = 0; i < time; i++)
            {
                searchRecordList.Add(DateTime.Now.AddDays(-29 + i).Date.ToShortDateString());
            }
        }
        else if(time == 365)
        {
            for(int i = 0; i < 12; i++)
            {
                int month = DateTime.Now.AddMonths(-12 + i).Month;
                if (month > 11) { month -= 12; }

                searchRecordList.Add(months[month]);
            }
        }

        return searchRecordList;
    }

    public List<Tuple<int, int>> buildBucketsForAccounts<T>(
        int time,
        List<string> labels,
        List<T> group,
        Func<T, int> predicate1,
        Func<T, DateTime> predicate3,
        Func<T, int> predicate4,
        Func<T, int> predicate5
    ) where T : AccountRecord
    {
        var searchRecordBuckets = new List<Tuple<int, int>>();
        List<string> months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        IEnumerable<T> query = null;
        
        if(time != -1)
        {
            if(time < 2 && time >= 0)
            {
                DateTime check = DateTime.Now.AtMidnight().AddDays(-time);
                searchRecordBuckets.Add(new Tuple<int, int>( 0, 0 ));
                query = group.Where(h => predicate1.Invoke(h) == check.DayOfYear);
                query.ToList().ForEach(h => {
                    searchRecordBuckets[0] = new Tuple<int, int>(
                        searchRecordBuckets[0].Item1 + h.AccountsCreated,
                        searchRecordBuckets[0].Item2 + h.AccountsDeleted
                    );
                });
            }
            else if(time > 0)
            {
                // Pre-initialize the searchRecordBuckets for a year.
                if(time == 365)
                {
                    for(int i = 0; i < 12; i++)
                    {
                        searchRecordBuckets.Add(new Tuple<int, int>( 0, 0 ));
                    }
                }

                foreach(var j in Enumerable.Range(0, time))
                {
                    if(time != 365)
                    {
                        searchRecordBuckets.Add(new Tuple<int, int>( 0, 0 ));

                        DateTime check = DateTime.Now.AtMidnight().AddDays((-time) + j + 1);
                        query = group.Where(h => predicate3.Invoke(h) == check.Date);
                        query.ToList().ForEach(h => {
                            searchRecordBuckets[j] = new Tuple<int, int>(
                                searchRecordBuckets[j].Item1 + h.AccountsCreated,
                                searchRecordBuckets[j].Item2 + h.AccountsDeleted
                            );
                        });
                    }
                    else
                    {
                        DateTime check = DateTime.Now.AtMidnight().AddDays((-time) + j + 1);

                        query = group.Where(h => predicate4.Invoke(h) == check.Month && predicate3.Invoke(h) == check.Date);
                        int currentMonth = DateTime.Now.Month;
                        int currentYear = DateTime.Now.Year;
                        int checkMonth = check.Month;
                        int checkYear = check.Year;
                        int index = ((checkYear - currentYear) * 12 + checkMonth - currentMonth + 11) % 12;

                        if (index < 0)
                        {
                            index += 12;
                        }

                        query.ToList().ForEach(h => {
                            searchRecordBuckets[index] = new Tuple<int, int>(
                                searchRecordBuckets[index].Item1 + h.AccountsCreated,
                                searchRecordBuckets[index].Item2 + h.AccountsDeleted
                            );
                        });
                    }
                }
            }
        }
        else
        {
            foreach(var i in Enumerable.Range(0, group.Count))
            {
                var element = group.ElementAt(i);
                string text = $"{months[predicate4.Invoke(element) - 1]} {predicate5.Invoke(element)}";
                if(labels.Contains(text))
                {
                    searchRecordBuckets[labels.IndexOf(text)] = new Tuple<int, int>(
                        searchRecordBuckets[labels.IndexOf(text)].Item1 + element.AccountsCreated,
                        searchRecordBuckets[labels.IndexOf(text)].Item2 + element.AccountsDeleted
                    );
                }
                else
                {
                    labels.Add(text);
                    searchRecordBuckets.Add(new Tuple<int, int>(element.AccountsCreated, element.AccountsDeleted));
                }
            }
        }

        return searchRecordBuckets;
    }

    public List<int> buildBuckets<T>(
        int time,
        List<string> labels,
        List<T> group,
        Func<T, int> predicate1,
        Func<T, int> predicate2,
        Func<T, DateTime> predicate3,
        Func<T, int> predicate4,
        Func<T, int> predicate5
    )
    {
        var searchRecordBuckets = new List<int>();
        List<string> months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        IEnumerable<T> query = null;
        
        if(time != -1)
        {
            if(time < 2 && time >= 0)
            {
                DateTime check = DateTime.Now.AtMidnight().AddDays(-time);
                foreach(var i in Enumerable.Range(0, 23))
                {
                    searchRecordBuckets.Add(0);
                    query = group.Where(h => predicate1.Invoke(h) == check.DayOfYear && predicate2.Invoke(h) == i);
                    query.ToList().ForEach(h => {
                        searchRecordBuckets[i]++;
                    });
                }
            }
            else if(time > 0)
            {
                // Pre-initialize the searchRecordBuckets for a year.
                if(time == 365)
                {
                    for(int i = 0; i < 12; i++)
                    {
                        searchRecordBuckets.Add(0);
                    }
                }

                foreach(var j in Enumerable.Range(0, time))
                {
                    if(time != 365)
                    {
                        searchRecordBuckets.Add(0);

                        DateTime check = DateTime.Now.AtMidnight().AddDays((-time) + j + 1);
                        query = group.Where(h => predicate3.Invoke(h) == check.Date);
                        query.ToList().ForEach(h => {
                            searchRecordBuckets[j]++;
                        });
                    }
                    else
                    {
                        DateTime check = DateTime.Now.AtMidnight().AddDays((-time) + j + 1);

                        query = group.Where(h => predicate4.Invoke(h) == check.Month && predicate3.Invoke(h) == check.Date);
                        int currentMonth = DateTime.Now.Month;
                        int currentYear = DateTime.Now.Year;
                        int checkMonth = check.Month;
                        int checkYear = check.Year;
                        int index = ((checkYear - currentYear) * 12 + checkMonth - currentMonth + 11) % 12;

                        if (index < 0)
                        {
                            index += 12;
                        }

                        query.ToList().ForEach(h => {
                            searchRecordBuckets[index]++;
                        });
                    }
                }
            }
        }
        else
        {
            foreach(var i in Enumerable.Range(0, group.Count))
            {
                var element = group.ElementAt(i);
                string text = $"{months[predicate4.Invoke(element) - 1]} {predicate5.Invoke(element)}";
                if(labels.Contains(text))
                {
                    searchRecordBuckets[labels.IndexOf(text)]++;
                }
                else
                {
                    labels.Add(text);
                    searchRecordBuckets.Add(1);
                }
            }
        }

        return searchRecordBuckets;
    }
}