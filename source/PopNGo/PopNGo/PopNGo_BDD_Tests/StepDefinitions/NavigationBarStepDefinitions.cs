using PopNGo.Utilities;
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



namespace PopNGo_BDD_Tests.StepDefinitions
{
    [Binding]
    public sealed class NavigationBarStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly IWebDriver _webDriver;

        public NavigationBarStepDefinitions()
        {
            _webDriver = new FirefoxDriver(); // Or use another browser driver
            _homePage = new HomePageObject(_webDriver);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }

        [Given("the user is on the home page")]
        public void GivenTheUserIsOnTheHomePage()
        {
            _webDriver.Navigate().GoToUrl(Common.UrlFor("Home"));

        }

        [Then("the user should see the navigation bar")]
        public void ThenTheUserShouldSeeTheNavigationBar()
        {
            _homePage.NavigationBar.Displayed.Should().BeTrue();
        }

        [When("the user clicks on the home logo")]
        public void WhenTheUserClicksOnTheHomeLogo()
        {
            _homePage.ClickHomeLogo();
        }

        [Then("the user should be directed to the home page")]
        public void ThenTheUserShouldBeDirectedToTheHomePage()
        {
            _webDriver.Url.Should().Be(Common.UrlFor("Home"));
        }

        [When("the user hovers over the home logo")]
        public void WhenTheUserHoversOverTheHomeLogo()
        {
            _homePage.GetHomeLogoColor();
        }

        [Then("the color of the home logo should not be white")]
        public void ThenTheColorOfTheHomeLogoShouldNotBeWhite()
        {
            var color = _homePage.GetHomeLogoColor();
            Assert.AreNotEqual("rgb(255, 255, 255)", color); // Check that the color is not white
        }
    }
}
