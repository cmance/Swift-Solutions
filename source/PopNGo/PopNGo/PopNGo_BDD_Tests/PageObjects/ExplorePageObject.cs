using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class ExplorePageObject : PageObject
    {
        public ExplorePageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Explore";
        }

        public IWebElement EventCard => _webDriver.FindElement(By.ClassName("event-info-container"));
        public IWebElement BuyTicketsDropdown => _webDriver.FindElement(By.Id("buyTicketsBtn"));
        public IWebElement ViewVenueButton => _webDriver.FindElement(By.Id("viewVenueBtn"));
        public IWebElement VenueInformationModal => _webDriver.FindElement(By.Id("view-venue-modal"));

        public void ClickEventCard()
        {
            EventCard.Click();
        }
    }
}