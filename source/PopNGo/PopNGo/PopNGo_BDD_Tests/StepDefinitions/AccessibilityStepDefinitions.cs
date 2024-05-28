using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;
using PopNGo_BDD_Tests.Drivers;
using OpenQA.Selenium.Chrome; // Add this line
using OpenQA.Selenium.Edge; // Add this line
using OpenQA.Selenium.Firefox; // Add this line
using System.Threading.Tasks;

namespace PopNGo_BDD_Tests.StepDefinitions
{
    [Binding]
    public sealed class AccessiblityStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly LoginPageObject _loginPage;
        private readonly IWebDriver _webDriver;
        private readonly Drivers.BrowserDriver _browserDriver;
        private readonly ScenarioContext _scenarioContext;

        public AccessiblityStepDefinitions(ScenarioContext context, Drivers.BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _webDriver = _browserDriver.Current; // Or use another browser driver
            _homePage = new HomePageObject(_webDriver);
            _loginPage = new LoginPageObject(_webDriver);
            _scenarioContext = context;
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [Given("the user is on the home page 2")]
        public void GivenTheUserIsOnTheHomePage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Home"));
        }

        [Then("all images should have an alt attribute and all buttons should have an aria label")]
        public void ThenAllImagesShouldHaveAnAltAttributeAndAllButtonsShouldHaveAnAriaLabel()
        {
            var images = _webDriver.FindElements(By.TagName("img"));
            foreach (var image in images)
            {
                var altAttribute = image.GetAttribute("alt");
                Assert.IsNotNull(altAttribute, $"Image with src '{image.GetAttribute("src")}', id '{image.GetAttribute("id")}', and class '{image.GetAttribute("class")}' does not have an alt attribute.");
            }

            var buttons = _webDriver.FindElements(By.TagName("button"));
            foreach (var button in buttons)
            {
                var ariaLabel = button.GetAttribute("aria-label");
                Assert.IsNotNull(ariaLabel, $"Button with text '{button.Text}', id '{button.GetAttribute("id")}', and class '{button.GetAttribute("class")}' does not have an aria-label attribute.");
            }
        }

        [Given("the user is on the explore page 2")]
        public void GivenTheUserIsOnTheExplorePage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Explore"));
        }

        [Given("the user is on the favorites page 2")]
        public void GivenTheUserIsOnTheFavoritesPage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Favorites"));
        }

        [Given("the user is on the history page 2")]
        public void GivenTheUserIsOnTheHistoryPage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("History"));
        }

        [Given("the user is on the itinerary page 2")]
        public void GivenTheUserIsOnTheItineraryPage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Itinerary"));
        }
        
        [Given("the user is on the profile page 2")]
        public void GivenTheUserIsOnTheProfilePage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Profile"));
        }

        [Given("the user is on the recommendation page 2")]
        public void GivenTheUserIsOnTheRecommendationPage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Recommendation"));
        }
    }
}