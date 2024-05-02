using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class ExplorePageObject : PageObject
    {
        private IWebElement EventCardElement;
        public ExplorePageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Explore";
        }

        public IWebElement EventsContainer => _webDriver.FindElement(By.Id("events-container"));
        public IWebElement EventCard => GetEventCard();
        public IWebElement BuyTicketsDropdown => _webDriver.FindElement(By.Id("buy-tickets-btn"));
        public IWebElement ViewVenueButton => _webDriver.FindElement(By.Id("view-venue-btn"));
        public IWebElement VenueInformationModal => _webDriver.FindElement(By.Id("view-venue-modal"));

        public SelectElement SelectCountry => new SelectElement(_webDriver.FindElement(By.Id("search-event-country")));
        public SelectElement SelectState => new SelectElement(_webDriver.FindElement(By.Id("search-event-state")));
        public SelectElement SelectCity => new SelectElement(_webDriver.FindElement(By.Id("search-event-city")));

        public IWebElement GetEventCard()
        {
            // WaitUntil<IWebElement>(() => locationCity, x => x.Text != "");
            while(EventsContainer.FindElements(By.Id("event-card-container")).Count == 0 &&
                EventsContainer.FindElements(By.ClassName("placeholder-style")).Count < 1)
            {
                Wait(100);                 
            }
            EventCardElement = EventsContainer.FindElement(By.Id("event-card-container"));
            return EventCardElement;
        }
        public void ClickEventCard()
        {
            
            EventCard.Click();
        }

        public void Wait(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }
    }
}