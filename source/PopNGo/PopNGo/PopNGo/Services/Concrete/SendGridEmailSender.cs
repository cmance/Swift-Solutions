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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmailSender(IConfiguration configuration,
                       ILogger<EmailSender> logger,
                       IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
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
        string baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

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