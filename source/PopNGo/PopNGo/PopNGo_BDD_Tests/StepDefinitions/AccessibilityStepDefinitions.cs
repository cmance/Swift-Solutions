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
        private readonly IWebDriver _webDriver;
        private readonly Drivers.BrowserDriver _browserDriver;
        private readonly ScenarioContext _scenarioContext;

        public AccessiblityStepDefinitions(ScenarioContext context, Drivers.BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _webDriver = _browserDriver.Current; // Or use another browser driver
            _homePage = new HomePageObject(_webDriver);
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
                Assert.IsNotNull(image.GetAttribute("alt"));
            }

            var buttons = _webDriver.FindElements(By.TagName("button"));
            foreach (var button in buttons)
            {
                Assert.IsNotNull(button.GetAttribute("aria-label"));
            }
        }

    }
}