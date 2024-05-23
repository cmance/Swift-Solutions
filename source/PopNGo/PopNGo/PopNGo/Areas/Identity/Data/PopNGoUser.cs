using Microsoft.AspNetCore.Identity;

namespace PopNGo.Areas.Identity.Data;

public class PopNGoUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }
    [PersonalData]
    public string NotificationEmail { get; set; }
    public bool NotifyWeekBefore { get; set; }
    public bool NotifyDayBefore { get; set; }
    public bool NotifyDayOf { get; set; }
    public string DistanceUnit { get; set; }
    public string TemperatureUnit { get; set; }
    public string MeasurementUnit { get; set; }
    public string ItineraryReminderTime { get; set; }
}