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
    public sealed class NavigationBarStepDefinitions
    {
        private readonly HomePageObject _homePage;
        private readonly IWebDriver _webDriver;

        public NavigationBarStepDefinitions(Drivers.BrowserDriver browserDriver)
        {
            _webDriver = browserDriver.Current;
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
            _homePage.OpenNavigationBar();
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

        [When("the user clicks on the hide button")]
        public async Task WhenTheUserClicksOnTheHideButton()
        {
            if (!_homePage.NavigationBar.Displayed)
            {
                _homePage.OpenNavigationBar();
                await Task.Delay(500);
            }
            
            _homePage.CloseNavigationBar();
            await Task.Delay(500);
        }

        [Then("the navigation bar should disappear")]
        public void ThenTheNavigationBarShouldDisappear()
        {
            _homePage.NavigationBar.Displayed.Should().BeFalse();
        }

        [When("the user clicks on the show button")]
        public async Task WhenTheUserClicksOnTheShowButton()
        {
            if (_homePage.NavigationBar.Displayed)
            {
                _homePage.CloseNavigationBar();
                await Task.Delay(500);
            }

            _homePage.OpenNavigationBar();
            await Task.Delay(500);
        }

        [Then("the navigation bar should appear")]
        public async Task ThenTheNavigationBarShouldAppear()
        {
            await Task.Delay(500); // Wait for the UI to update
            _homePage.NavigationBar.Displayed.Should().BeTrue();
        }
    }
}
