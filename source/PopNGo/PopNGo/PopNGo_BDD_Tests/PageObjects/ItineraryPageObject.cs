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

        public IWebElement FirstItineraryToggle => _webDriver.FindElement(By.Id("accordion-header-bg"));
        public IWebElement FirstItinerary => _webDriver.FindElement(By.Id("heading0"));
        public IWebElement FirstEvent => _webDriver.FindElement(By.ClassName("single-timeline-area"));
        public SelectElement ReminderSelect => new SelectElement(FirstEvent.FindElement(By.Id("reminder-time-select")));
        public IWebElement ReminderButton => FirstEvent.FindElement(By.Id("save-reminder-button"));
        public IWebElement ReminderTime => FirstEvent.FindElement(By.Id("reminder-time"));
        public IWebElement ScenarioEvent => FindEventByTitle();
        public SelectElement ScenarioEventReminder => new SelectElement(ScenarioEvent.FindElement(By.Id("reminder-time-select")));
        public IWebElement ScenarioEventReminderButton => ScenarioEvent.FindElement(By.Id("save-reminder-button"));

        public void SetEventTitle(string title)
        {
            eventTitle = title;
        }

        public IWebElement FindEventByTitle()
        {
            // Finds the event title, then returns the parent hierarchy
            return _webDriver.FindElement(By.XPath($"//h5[@id='event-title' and contains(text(), '{eventTitle}')]")).FindElement(By.XPath(".."));
        }
    }
}