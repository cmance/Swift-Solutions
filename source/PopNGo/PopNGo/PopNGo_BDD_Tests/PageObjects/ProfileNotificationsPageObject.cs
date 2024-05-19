using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class ProfileNotificationsPageObject : PageObject
    {
        public ProfileNotificationsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Profile Notifications";
        }

        public SelectElement ItineraryDefaultTime => new SelectElement(_webDriver.FindElement(By.Id("Input_ItineraryReminderTime")));

        public IWebElement SaveButton => _webDriver.FindElement(By.Id("update-notifications-button"));
    }
}