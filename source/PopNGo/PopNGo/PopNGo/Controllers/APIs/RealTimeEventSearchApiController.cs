using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;
using PopNGo.Models;
using PopNGo.Services;

namespace PopNGo.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RealTimeEventSearchApiController : ControllerBase
    {
        private readonly IRealTimeEventSearchService _realTimeEventSearchService;
        private readonly IEventRepository _eventRepository;
        private readonly ISearchRecordRepository _searchRecordRepository;
        private readonly IPgUserRepository _pgUserRepository;
        private readonly IEventTagRepository _eventTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<RealTimeEventSearchApiController> _logger;
        private readonly UserManager<PopNGoUser> _userManager;
        public RealTimeEventSearchApiController(
            IRealTimeEventSearchService realTimeEventSearchService,
            IEventRepository eventRepository,
            ITagRepository tagRepository,
            IEventTagRepository eventTagRepository,
            ISearchRecordRepository searchRecordRepository,
            IPgUserRepository pgUserRepository,
            ILogger<RealTimeEventSearchApiController> logger,
            UserManager<PopNGoUser> userManager)
        {
            _realTimeEventSearchService = realTimeEventSearchService;
            _eventRepository = eventRepository;
            _eventTagRepository = eventTagRepository;
            _tagRepository = tagRepository;
            _searchRecordRepository = searchRecordRepository;
            _pgUserRepository = pgUserRepository;
            _logger = logger;
            _userManager = userManager;
        }
        // GET: api/search/events/?q=Q&start=0
        [HttpGet("search/events")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EventDetail>))]
        public async Task<ActionResult<IEnumerable<PopNGo.Models.DTO.Event>>> GetRealTimeAPIEvents(string q, int start, string date)
        {
            try
            {
                PgUser user = _pgUserRepository.GetPgUserFromIdentityId(_userManager.GetUserId(User));

                _searchRecordRepository.AddOrUpdate(new SearchRecord
                {
                    SearchQuery = q,
                    Time = DateTime.Now,
                    UserId = user?.Id ?? 0
                });

                var events = new List<PopNGo.Models.DTO.Event>();
                try {
                    IEnumerable<EventDetail> eventsDetails = await _realTimeEventSearchService.SearchEventAsync(q, start, date);
                    // Save events to database
                    for (int i = 0; i < eventsDetails.Count(); i++)
                    {
                        EventDetail eventDetail = eventsDetails.ElementAt(i);
                        if (!_eventRepository.IsEvent(eventDetail.EventID))
                        {
                            Event addedEvent = _eventRepository.AddEvent(eventDetail);
                            // Add tags to database
                            foreach (string tag in eventDetail.EventTags)
                            {
                                // Skip any tags that are too long
                                if(tag.Length <= 255) {
                                    Models.Tag foundTag = await _tagRepository.FindByName(tag);
                                    if (foundTag == null) {
                                        foundTag = await _tagRepository.CreateNew(tag);
                                    }
                                
                                    try {
                                        // Add event tags to database
                                        _eventTagRepository.AddEventTag(foundTag.Id, addedEvent.Id);
                                    } catch (Exception e) {
                                        Console.WriteLine("Error adding event tag: " + e.Message);
                                    }
                                }
                            }
                        }
                    }
                    
                    events = _eventRepository.GetEventsFromEventApiIds(eventsDetails.Select(e => e.EventID).ToList());
                } catch (Exception e) {
                    Console.WriteLine("Error adding event: " + e.Message);
                }
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching movies repositories");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
