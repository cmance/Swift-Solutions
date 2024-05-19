using System.Drawing.Drawing2D;
using System.Reflection.PortableExecutable;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PopNGo.Areas.Identity.Data;
using PopNGo.DAL.Abstract;
using PopNGo.Models;

namespace PopNGo.Services;

public class EmailBuilder
{
    private readonly IPgUserRepository _userRepo;
    private readonly IFavoritesRepository _favoritesRepo;
    private readonly IBookmarkListRepository _bookmarkListRepo;
    private readonly IItineraryRepository _itineraryRepository;
    private readonly UserManager<PopNGoUser> _userManager;

    public EmailBuilder(
        IPgUserRepository userRepo,
        IFavoritesRepository favoritesRepo,
        IBookmarkListRepository bookmarkListRepo,
        IItineraryRepository itineraryRepository,
        UserManager<PopNGoUser> userManager)
    {
        _userRepo = userRepo;
        _favoritesRepo = favoritesRepo;
        _bookmarkListRepo = bookmarkListRepo;
        _itineraryRepository = itineraryRepository;
        _userManager = userManager;
    }

    public async Task<string> BuildUpcomingEventsEmailAsync(int userId)
    {
        PgUser user = _userRepo.FindById(userId);
        PopNGoUser popNGoUser = await _userManager.FindByIdAsync(user.AspnetuserId);
        string emailBody = "";

        // If the user has no linked account, don't send the email
        if (popNGoUser != null)
        {
            List<Models.DTO.Event> weekBeforeFaves = new List<Models.DTO.Event>();
            List<Models.DTO.Event> dayBeforeFaves = new List<Models.DTO.Event>();
            List<Models.DTO.Event> dayOfFaves = new List<Models.DTO.Event>();

            List<Models.DTO.BookmarkList> bookmarks = _bookmarkListRepo.GetBookmarkLists(user.Id);
            foreach (var bookmark in bookmarks)
            {
                List<Models.DTO.Event> favorites = _favoritesRepo.GetUserFavorites(bookmark.Id);

                // Get the current date
                // And populate the lists with the events that match the criteria
                DateTime now = DateTime.Now.Date;
                foreach (var favorite in favorites)
                {
                    DateTime eventStart = favorite.EventDate.Date;
                    if (eventStart == now.AddDays(7) && popNGoUser.NotifyWeekBefore)
                    {
                        weekBeforeFaves.Add(favorite);
                    }
                    else if (eventStart == now.AddDays(1) && popNGoUser.NotifyDayBefore)
                    {
                        dayBeforeFaves.Add(favorite);
                    }
                    else if (eventStart == now && popNGoUser.NotifyDayOf)
                    {
                        dayOfFaves.Add(favorite);
                    }
                }
            }

            // Build the email body
            emailBody = $"<div style=\"margin: 0px; padding: 0px;\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">";

            // Today's events
            emailBody += BuildEventsSection(dayOfFaves, "Today");

            // Tomorrow's events
            emailBody += BuildEventsSection(dayBeforeFaves, "Tomorrow");

            // Events from a week from now
            emailBody += BuildEventsSection(weekBeforeFaves, "A week from now");

            // Close the email
            emailBody += "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";
            emailBody += "</table></div>";
        }

        return emailBody;
    }

    public string BuildEventsSection(List<Models.DTO.Event> events, string sectionTitle)
    {
        string sectionContent = "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";
        sectionContent += $"<tr><td width=\"100%\" align=\"center\"><h2 style=\"color: #ffffff; font-family: Arial, sans-serif;\">{sectionTitle}:</h3></td></tr>";
        if (events.Count > 0)
        {
            sectionContent += BuildEvents(events);
        }
        else
        {
            sectionContent += "<tr><td width=\"100%\" align=\"center\"><p style=\"color: #ffffff; font-family: Arial, sans-serif;\">No events to remind you about</p></td></tr>";
        }

        return sectionContent;
    }
    public string BuildEvents(List<Models.DTO.Event> events)
    {
        string emailBody = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"1\" width=\"90%\" align=\"center\" bgcolor=\"#ff8f45\" style=\"border-color: rgba(163, 65, 0, 0.5); border-radius: 5px; margin-bottom:10px;\">";
        foreach (var e in events)
        {
            if (e.EventDate.Hour == 0)
            {
                e.EventDate = e.EventDate.AddHours(12);
            }
            string time = e.EventDate.TimeOfDay.Hours > 12 ? $"{e.EventDate.TimeOfDay.Hours - 12}:{e.EventDate.TimeOfDay.Minutes.ToString("D2")} PM" : $"{e.EventDate.TimeOfDay.Hours}:{e.EventDate.TimeOfDay.Minutes.ToString("D2")} AM";
            string locationURL = $"https://www.google.com/maps/place/{e.EventLocation.Replace(" ", "+")}";
            emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px;\"><h3 style=\"color: #a73f00; font-family: Arial, sans-serif;\">{e.EventName} - {time}</h4></td></tr>";
            emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px; padding-bottom: 5px;\"><a href=\"{locationURL}\">{e.EventLocation}</a></td></tr>";
            emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px; padding-bottom: 10px;\"><p style=\"color: #a73f00; font-family: Arial, sans-serif; margin: 0px\">{e.EventDescription}</p></td></tr>";

            if (events.IndexOf(e) < events.Count - 1)
            {
                emailBody += "<tr><td style=\"border-width:0px; border-bottom: 1px solid #d6d6d6; line-height: 1px;\" width=\"100%\" align=\"center\"></td></tr>";
            }
        }
        emailBody += "</table>";
        return emailBody;
    }

    public Tuple<string, string> BuildItineraryEventEmail(int itineraryId, string eventId)
    {
        Itinerary itinerary = _itineraryRepository.FindById(itineraryId);
        List<Event> events = itinerary.ItineraryEvents.Select(e => e.Event).ToList();
        Event eventToHighlight = events.FirstOrDefault(e => e.ApiEventId == eventId);
        int eventIndex = events.IndexOf(eventToHighlight);

        string emailBody = "";
        string time = "";

        emailBody += $"<tr><td width=\"100%\" align=\"center\"><h2 style=\"color: #ffffff; font-family: Arial, sans-serif;\">{itinerary.ItineraryTitle}:</h3></td></tr>";

        // Build the email body
        emailBody = $"<div style=\"margin: 0px; padding: 0px;\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">";

        for(int i = 0; i < events.Count; i++)
        {
            Event e = events[i];
            time = e.EventDate.Value.TimeOfDay.Hours > 12 ? $"{e.EventDate.Value.TimeOfDay.Hours - 12}:{e.EventDate.Value.TimeOfDay.Minutes.ToString("D2")} PM" : $"{e.EventDate.Value.TimeOfDay.Hours}:{e.EventDate.Value.TimeOfDay.Minutes.ToString("D2")} AM";

            if(i < eventIndex)
            {
                emailBody += $"<tr><td width=\"100%\" align=\"center\" style=\"border-width: 0px; padding-left: 10px;\"><h3 style=\"color: #a73f00; text-decoration: line-through; font-family: Arial, sans-serif;\">{e.EventName} - {time}</h4></td></tr>";
            }
            else if(i == eventIndex)
            {
                emailBody += $"<tr><td width=\"100%\" align=\"center\" style=\"border-width: 0px; padding-left: 10px;\"><h3 style=\"color: #a73f00; font-weight: bold; font-family: Arial, sans-serif;\">{e.EventName} - {time}</h4></td></tr>";
            }
            else
            {
                emailBody += $"<tr><td width=\"100%\" align=\"center\" style=\"border-width: 0px; padding-left: 10px;\"><h3 style=\"color: #a73f00; font-family: Arial, sans-serif;\">{e.EventName} - {time}</h4></td></tr>";
            }
        }

        emailBody += "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";

        emailBody += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"1\" width=\"90%\" align=\"center\" bgcolor=\"#ff8f45\" style=\"border-color: rgba(163, 65, 0, 0.5); border-radius: 5px; margin-bottom:10px;\">";

        if (events[eventIndex].EventDate.Value.Hour == 0)
        {
            events[eventIndex].EventDate = events[eventIndex].EventDate.Value.AddHours(12);
        }

        time = events[eventIndex].EventDate.Value.TimeOfDay.Hours > 12 ? $"{events[eventIndex].EventDate.Value.TimeOfDay.Hours - 12}:{events[eventIndex].EventDate.Value.TimeOfDay.Minutes.ToString("D2")} PM" : $"{events[eventIndex].EventDate.Value.TimeOfDay.Hours}:{events[eventIndex].EventDate.Value.TimeOfDay.Minutes.ToString("D2")} AM";
        string locationURL = $"https://www.google.com/maps/place/{events[eventIndex].EventLocation.Replace(" ", "+")}";
        emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px;\"><h3 style=\"color: #a73f00; font-family: Arial, sans-serif;\">{events[eventIndex].EventName} - {time}</h4></td></tr>";
        emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px; padding-bottom: 5px;\"><a href=\"{locationURL}\">{events[eventIndex].EventLocation}</a></td></tr>";
        emailBody += $"<tr><td width=\"100%\" style=\"border-width: 0px; padding-left: 10px; padding-bottom: 10px;\"><p style=\"color: #a73f00; font-family: Arial, sans-serif; margin: 0px\">{events[eventIndex].EventDescription}</p></td></tr>";
        emailBody += "</table>";

        // Close the email
        emailBody += "<hr style=\"border-top: 1px solid #d6d6d6; line-height: 1px;\" width=\"90%\" align=\"center\" />";
        emailBody += "</table></div>";

        Console.WriteLine(emailBody);
        return new Tuple<string, string>(emailBody, itinerary.ItineraryTitle);
    }

}