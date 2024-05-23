using Microsoft.AspNetCore.Mvc;
using PopNGo.Models;
using PopNGo.DAL.Abstract;
using PopNGo.DAL.Concrete;

using Microsoft.AspNetCore.Identity;
using PopNGo.Areas.Identity.Data;
using PopNGo.Models.DTO;
using PopNGo.ExtensionMethods;
using PopNGo.Services;

namespace PopNGo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RecommendationsApiController : Controller
{
    private readonly ILogger<RecommendationsApiController> _logger;
    private readonly IRealTimeEventSearchService _realTimeEventSearchService;

    private readonly IConfiguration _configuration;
    private readonly IPgUserRepository _pgUserRepository;
    private readonly UserManager<PopNGoUser> _userManager;
    private readonly IOpenAiService _openAiService;
    private readonly IEventRepository _eventRepository;
    private readonly IEventTagRepository _eventTagRepository;
    private readonly IFavoritesRepository _favoritesRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IRecommendedEventRepository _recommendedEventRepository;
    private readonly IEventHistoryRepository _eventHistoryRepository;

    public RecommendationsApiController(
        IPgUserRepository pgUserRepository,
        UserManager<PopNGoUser> userManager,
        ILogger<RecommendationsApiController> logger,
        IConfiguration configuration,
        IEventHistoryRepository eventHistoryRepository,
        IEventRepository eventRepository,
        ITagRepository tagRepository,
        IEventTagRepository eventTagRepository,
        IFavoritesRepository favoritesRepository,
        IRealTimeEventSearchService realTimeEventSearchService,
        IRecommendedEventRepository recommendedEventRepository,
        IOpenAiService openAiService
    )
    {
        _logger = logger;
        _configuration = configuration;
        _pgUserRepository = pgUserRepository;
        _userManager = userManager;
        _openAiService = openAiService;
        _favoritesRepository = favoritesRepository;
        _eventHistoryRepository = eventHistoryRepository;
        _eventTagRepository = eventTagRepository;
        _tagRepository = tagRepository;
        _eventRepository = eventRepository;
        _recommendedEventRepository = recommendedEventRepository;
        _realTimeEventSearchService = realTimeEventSearchService;
    }

    [HttpGet("RecommendedEvents")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PopNGo.Models.Event>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PopNGo.Models.DTO.Event>>> RecommendedEvents(string location)
    {
        PopNGoUser user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        PgUser pgUser = _pgUserRepository.GetPgUserFromIdentityId(user.Id);
        if (pgUser == null)
        {
            return Unauthorized();
        }

        var isYesterday = DateTime.Today - pgUser.RecommendedPreviouslyAt?.Date >= TimeSpan.FromDays(1);
        // Check user's last recommendation date and only recommend if recommendation was yesterday or earlier
        if (!isYesterday)
        {
            return Ok(_recommendedEventRepository.GetRecommendedEvents(pgUser.Id));
        }

        // Grab all user's favorited and history events
        List<Models.DTO.Event> favorites = _favoritesRepository.GetAllFavorites(pgUser.Id);
        List<Models.DTO.Event> history = _eventHistoryRepository.GetEventHistory(pgUser.Id);
        // Combine the two lists and remove duplicates
        List<Models.DTO.Event> combined = favorites.Concat(history).Distinct().ToList();
        // Pick 5 random events from the combined list
        List<Models.DTO.Event> randomEvents = combined.OrderBy(x => Guid.NewGuid()).Take(5).ToList();

        // Get the most relevant words from each of the random events title + description. Use Task.WhenAll to run all tasks concurrently
        List<Task<string>> relevantWordsTasks = randomEvents.Select(async e => await _openAiService.FindMostRelevantWordFromString(e.EventName + " " + e.EventDescription)).ToList();
        await Task.WhenAll(relevantWordsTasks);
        
        // Resolve the tasks to get the results
        List<string> relevantWords = relevantWordsTasks.Select(t => t.Result).ToList();
        // Remove any null or empty strings
        relevantWords.RemoveAll(rw => string.IsNullOrEmpty(rw));
        // Add one empty string to the list so some aditional events are added
        relevantWords.Add("");

        // Make a query to the real-time api service for each relevant word, combine the results, remove duplicates. Use Task.WaitAll to run all tasks concurrently
        List<EventDetail> eventsDetails = new List<EventDetail>();
        List<Task<IEnumerable<EventDetail>>> searchTasks = new List<Task<IEnumerable<EventDetail>>>();
        for (int i = 0; i < relevantWords.Count; i++)
        {
            string relevantWordTaskResult = relevantWords[i];
            try
            {
                // Date is randomly 'today' or 'tomorrow'
                string date = new Random().Next(0, 2) == 0 ? "today" : "tomorrow";

                Task<IEnumerable<EventDetail>> searchTask = _realTimeEventSearchService.SearchEventAsync(relevantWordTaskResult + " events in " + location, 0, date);
                searchTasks.Add(searchTask);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding event: " + e.Message);
            }
        }

        await Task.WhenAll(searchTasks);
        foreach (var searchTask in searchTasks)
        {
            try
            {
                IEnumerable<EventDetail> newEventDetails = await searchTask;
                eventsDetails.AddRange(newEventDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding event: " + e.Message);
            }
        }

        // Remove duplicates from the events details
        eventsDetails = eventsDetails.Distinct().ToList();

        // Save events to database
        for (int ed_i = 0; ed_i < eventsDetails.Count(); ed_i++)
        {
            EventDetail eventDetail = eventsDetails.ElementAt(ed_i);
            if (!_eventRepository.IsEvent(eventDetail.EventID))
            {
                PopNGo.Models.Event addedEvent = _eventRepository.AddEvent(eventDetail);
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

        // Get the event api ids from the event details
        List<string> eventApiIds = eventsDetails.Select(ed => ed.EventID).ToList();

        // Get the events from the event api ids
        List<PopNGo.Models.DTO.Event> events = _eventRepository.GetEventsFromEventApiIds(eventApiIds);

        // Select up to 20 random events from the combined list of events
        List<PopNGo.Models.DTO.Event> randomEventsFromCombined = events.OrderBy(x => Guid.NewGuid()).Take(20).ToList();

        // Set user's recommended events to the 20 random events
        _recommendedEventRepository.SetRecommendedEvents(pgUser.Id, randomEventsFromCombined.Select(e => e.ApiEventID).ToList());

        // Set user's last recommended date to today
        _pgUserRepository.SetRecommendationsPreviouslyAtDate(pgUser.Id, DateTime.Now.Date);

        // Get the recommended events from the database
        List<PopNGo.Models.DTO.Event> recommendedEvents = _recommendedEventRepository.GetRecommendedEvents(pgUser.Id);

        return recommendedEvents;
    }
}
