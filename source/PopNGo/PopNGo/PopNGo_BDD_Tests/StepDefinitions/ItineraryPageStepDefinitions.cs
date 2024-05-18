using OpenQA.Selenium;
using PopNGo_BDD_Tests.PageObjects;
using PopNGo_BDD_Tests.Drivers;

namespace PopNGo_BDD_Tests.StepDefinitions
{
    [Binding]
    public sealed class ItineraryPageStepDefinitions
    {
        private readonly ItineraryPageObject _itineraryPage;
        private readonly ProfileNotificationsPageObject _profileNotificationsPage;
        private readonly ExplorePageObject _explorePage;
        private readonly IWebDriver _webDriver;
        private readonly BrowserDriver _browserDriver;
        private readonly ScenarioContext _scenarioContext;


        public ItineraryPageStepDefinitions(ScenarioContext context, BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _webDriver = _browserDriver.Current; // Or use another browser driver
            _itineraryPage = new ItineraryPageObject(_webDriver);
            _profileNotificationsPage = new ProfileNotificationsPageObject(_webDriver);
            _explorePage = new ExplorePageObject(_webDriver);
            _scenarioContext = context;
        }

        [Given(@"I am viewing my first itinerary")]
        [When(@"I am viewing my first itinerary")]
        public void GivenIAmViewingMyFirstItinerary()
        {
            _itineraryPage.FirstItineraryToggle.Click();
        }

        [Given(@"I look at the first event")]
        [When(@"I look at the first event")]
        public void GivenILookAtTheFirstEvent()
        {
            _itineraryPage.FirstEvent.Displayed.Should().BeTrue();
        }

        [Then(@"I can configure the time to send a reminder notification")]
        public void ThenICanConfigureTheTimeToSendAReminderNotification()
        {
            _itineraryPage.ReminderSelect.WrappedElement.Displayed.Should().BeTrue();
        }

        [When(@"I try to configure a time past the event's start")]
        public void WhenITryToConfigureATimePastTheEventsStart()
        {
            _itineraryPage.ReminderSelect.SelectByValue("custom");
            _itineraryPage.ReminderTime.SendKeys("12:01 AM");
        }

        [Then(@"the time is not changed from what it originally was")]
        public void ThenTheTimeIsNotChangedFromWhatItOriginallyWas()
        {
            _itineraryPage.ReminderTime.GetAttribute("value").Should().Be("12:00 AM");
        }

        [Then(@"I can see a default itinerary reminder time setting")]
        public void ThenICanSeeADefaultItineraryReminderTimeSetting()
        {
            _profileNotificationsPage.ItineraryDefaultTime.WrappedElement.Displayed.Should().BeTrue();
        }

        [When(@"I change the default itinerary reminder time")]
        public void WhenIChangeTheDefaultItineraryReminderTime()
        {
            _profileNotificationsPage.ItineraryDefaultTime.SelectByValue("half-hour");
            _profileNotificationsPage.SaveButton.Click();

            // Wait for the page to reload
            Thread.Sleep(3000);
            _scenarioContext["itineraryDefaultTime"] = "half-hour";
        }

        [When(@"I click the ""([^\""]*)"" button")]
        public void WhenIClickTheButton(string buttonName)
        {
            switch (buttonName)
            {
                case "Add to Itinerary":
                    _explorePage.AddToItinerary.Click();
                    break;
                default:
                    throw new Exception($"Button {buttonName} not recognized");
            }
        }

        [When(@"I click the first itinerary")]
        public void WhenIClickTheFirstItinerary()
        {
            _explorePage.ItineraryList.FindElement(By.ClassName("itinerary")).Click();
            _scenarioContext["itineraryEventTitle"] = _explorePage.EventCard.FindElement(By.Id("event-card-title")).Text;
        }

        [Then(@"I can see the new reminder time on the itinerary for this event")]
        public void ThenICanSeeTheNewReminderTimeOnTheItineraryForThisEvent()
        {
            _itineraryPage.SetEventTitle(_scenarioContext["itineraryEventTitle"].ToString());
            _itineraryPage.ScenarioEventReminder.SelectedOption.Text.Should().Be("30 minutes");
        }
    }
}