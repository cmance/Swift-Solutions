using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class ItineraryPageObject : PageObject
    {
        private string eventTitle;

        public ItineraryPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Itinerary";
        }

        public IWebElement FirstItineraryToggle => _webDriver.FindElement(By.Id("itinerary-toggle"));
        public IWebElement FirstItinerary => _webDriver.FindElement(By.ClassName("itinerary"));
        public IWebElement FirstEvent => FirstItinerary.FindElement(By.ClassName("event"));
        public SelectElement ReminderSelect => new SelectElement(FirstEvent.FindElement(By.Id("reminder-time-select")));
        public IWebElement ReminderTime => FirstEvent.FindElement(By.Id("reminder-time"));
        public IWebElement ScenarioEvent => FindEventByTitle();
        public SelectElement ScenarioEventReminder => new SelectElement(ScenarioEvent.FindElement(By.Id("reminder-time-select")));

        public void SetEventTitle(string title)
        {
            eventTitle = title;
        }

        public IWebElement FindEventByTitle()
        {
            return _webDriver.FindElement(By.XPath($"//div[@class='event' and contains(text(), '{eventTitle}')]"));
        }
    }
}