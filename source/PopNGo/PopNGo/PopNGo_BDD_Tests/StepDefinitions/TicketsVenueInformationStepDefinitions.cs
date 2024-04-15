using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;
using PopNGo_BDD_Tests.Drivers;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions; // Add this line

namespace PopNGo_BDD_Tests.StepDefinitions
{
    [Binding]
    public sealed class TicketsVenueInformationStepDefinitions
    {
        private readonly ExplorePageObject _explorePage;
        private readonly HomePageObject _homePage;
        private readonly IWebDriver _webDriver;
        private readonly BrowserDriver _browserDriver;

        public TicketsVenueInformationStepDefinitions(Drivers.BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _webDriver = _browserDriver.Current;
            _explorePage = new ExplorePageObject(_webDriver);
            _homePage = new HomePageObject(_webDriver);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [Given(@"the visitor navigates to the explore page")]
        public void GivenTheVisitorNavigatesToTheExplorePage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Explore"));
        }

        [When(@"the visitor clicks on any event card")]
        public void WhenTheVisitorClicksOnAnyEventCard()
        {
            _explorePage.ClickEventCard();
        }

        [Then(@"the visitor should see a modal with a ""(.*)"" and ""(.*)"" button")]
        public void ThenTheVisitorShouldSeeAModalWithAAndButton(string p0, string p1)
        {
            _explorePage.ViewVenueButton.Text.Should().Be(p0);
            _explorePage.BuyTicketsDropdown.Text.Should().Be(p1);

        }

        [When(@"the visitor clicks on the ""(.*)"" button")]
        public void WhenTheVisitorClicksOnTheButton(string p0)
        {
            IWebElement element = null;
            if(p0 == "View Venue")
                element = _explorePage.ViewVenueButton;
            else if(p0 == "Buy Tickets")
                element = _explorePage.BuyTicketsDropdown;
            
            _browserDriver.ScrollToElement(element);
            element.Click();
        }

        [Then(@"the visitor should be shown the venue information modal")]
        public void ThenTheVisitorShouldBeShownTheVenueInformationModal()
        {
            _explorePage.VenueInformationModal.Displayed.Should().BeTrue();
        }

        [Then(@"the visitor should activate a dropdown with ticket options or the button will be disabled if no ticket options are available")]
        public void ThenTheVisitorShouldActivateADropdownWithTicketOptionsOrTheButtonWillBeDisabledIfNoTicketOptionsAreAvailable()
        {
            _explorePage.BuyTicketsDropdown.Displayed.Should().BeTrue();
        }

        [When(@"the visitor hovers over the ""(.*)"" button")]
        public void WhenTheVisitorHoversOverTheButton(string p0)
        {
            _browserDriver.ScrollToElement(_explorePage.BuyTicketsDropdown);
            Actions actions = new Actions(_webDriver);
            actions.MoveToElement(_explorePage.BuyTicketsDropdown).Perform();
        }

        [Then(@"the button should have a button title appear")]
        public void ThenTheButtonShouldHaveAButtonTitleAppear()
        {
            _explorePage.BuyTicketsDropdown.GetAttribute("title").Should().NotBeNullOrEmpty();
        }

    }
}


