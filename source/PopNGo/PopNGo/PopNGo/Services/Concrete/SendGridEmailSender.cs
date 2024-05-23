using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PopNGo.Services;

public class EmailSender
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration,
                       ILogger<EmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, Dictionary<string, string> args)
    {
        if (string.IsNullOrEmpty(_configuration["SendGridAPIKey"]))
        {
            throw new Exception("Null SendGridAPIKey");
        }
        await Execute(_configuration["SendGridAPIKey"], subject, toEmail, args);
    }

    public async Task Execute(string apiKey, string subject, string toEmail, Dictionary<string, string> args)
    {
        var client = new SendGridClient(apiKey);

        var msg = new SendGridMessage()
        {
            From = new EmailAddress("popngo.wou@gmail.com", "PopNGo Admin"),
            Subject = subject,
        };

        var templateData = new Dictionary<string, string>();

        switch(args["template"])
        {
            case "confirmation":
                msg.TemplateId = "d-1d6c26b35471473780eb39a0a1572be0";
                string baseUrl = _configuration["BaseUrl"];

                templateData.Add("confirmURL", HtmlEncoder.Default.Encode(args["confirmationURL"]).Replace("amp;", ""));
                templateData.Add("exploreURL", $"{baseUrl}/Explore");
                templateData.Add("favoritesURL", $"{baseUrl}/Favorites");
                templateData.Add("itineraryURL", $"{baseUrl}/Itinerary");
                break;
            case "resetPassword":
                msg.TemplateId = "d-10a931a1542a4993873680452036cc72";
                templateData.Add("resetPasswordURL", HtmlEncoder.Default.Encode(args["resetPasswordURL"]).Replace("amp;", ""));
                break;
            case "upcomingEvents":
                msg.TemplateId = "d-02995c10569d4594b6e10f6e0445dd24";
                templateData.Add("messageContent", args["messageContent"]);
                templateData.Add("name", args["name"]);
                break;
            case "itineraryEvent":
                msg.TemplateId = "d-25bf67053a0341f1b821a3c167e30a86";
                templateData.Add("messageContent", args["messageContent"]);
                templateData.Add("name", args["name"]);
                templateData.Add("itineraryName", args["itineraryName"]);
                break;
            case "itineraryNotificationRemoval":
                msg.TemplateId = "d-5891c15024a641af8153be44c326ffe0";
                templateData.Add("messageContent", args["messageContent"]);
                templateData.Add("itineraryTitle", args["itineraryTitle"]);
                break;
            case "itineraryNotificationAddition":
                msg.TemplateId = "d-ad5de83dc91b443987fb78215fe595cc";
                templateData.Add("messageContent", args["messageContent"]);
                templateData.Add("itineraryTitle", args["itineraryTitle"]);
                templateData.Add("optOutURL", args["optOutURL"]);
                break;
            case "itineraryNotification":
                msg.TemplateId = "d-81f71e08eacf47aa90d847e5055a78f3";
                templateData.Add("messageContent", args["messageContent"]);
                templateData.Add("itineraryName", args["itineraryName"]);
                break;
        }

        msg.SetTemplateData(templateData);
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode 
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}: {response.StatusCode} {response.Body.ReadAsStringAsync().Result}");
    }
}