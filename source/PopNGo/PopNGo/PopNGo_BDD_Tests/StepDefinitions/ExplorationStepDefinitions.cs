using OpenQA.Selenium;
using PopNGo_BDD_Tests.PageObjects;

namespace PopNGo_BDD_Tests.StepDefinitions
{
    [Binding]
    public sealed class ExplorationStepDefinitions
    {
        private readonly ExplorePageObject _explorePage;
        private readonly IWebDriver _webDriver;
        private readonly Drivers.BrowserDriver _browserDriver;
        private readonly ScenarioContext _scenarioContext;


        public ExplorationStepDefinitions(ScenarioContext context, Drivers.BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _webDriver = _browserDriver.Current; // Or use another browser driver
            _explorePage = new ExplorePageObject(_webDriver);
            _scenarioContext = context;
        }

        [Given(@"I'm located in ""([^\""]*)"", ""([^\""]*)"", ""([^\""]*)""")]
        public void GivenImLocatedInCityStateCountry(string city, string state, string country)
        {
            _explorePage.SelectCountry.SelectByValue(country);
            Thread.Sleep(1000);
            _explorePage.SelectState.SelectByValue(state);
            Thread.Sleep(1000);
            _explorePage.SelectCity.SelectByValue(city);
            Thread.Sleep(1000);
        }

        // This step definition uses Cucumber Expressions. See https://github.com/gasparnagy/CucumberExpressions.SpecFlow
        [When("the events have loaded")]
        public void WhenTheEventsHaveLoaded()
        {
            // This will grab the first event card, as long as it's not a placeholder card.
            _explorePage.GetEventCard();
        }

        // This step definition uses Cucumber Expressions. See https://github.com/gasparnagy/CucumberExpressions.SpecFlow
        [Then(@"I should see section on the event cards with the id '([^\']*)'")]
        public void ThenIShouldSeeSectionOnTheEventCardsWithTheId(string id)
        {
            _explorePage.EventCard.FindElement(By.Id(id)).Displayed.Should().BeTrue();
        }
    }
}
