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
    public sealed class BookmarkListsStepDefinitions
    {
        private readonly FavoritesPageObject _favoritesPage;
        private readonly IWebDriver _webDriver;
        private readonly Drivers.BrowserDriver _browserDriver;
        private readonly ScenarioContext _scenarioContext;


        public BookmarkListsStepDefinitions(ScenarioContext context, Drivers.BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _webDriver = _browserDriver.Current; // Or use another browser driver
            _favoritesPage = new FavoritesPageObject(_webDriver);
            _scenarioContext = context;
        }

        [Then("I should see a bookmark list")]
        public void ThenIShouldSeeABookmarkList()
        {
            // Check that there is at least one bookmark list
            _favoritesPage.BookmarkLists.Count.Should().BeGreaterThan(0);
        }

        [Then("I should see a way to create a new bookmark list")]
        public void ThenIShouldSeeAWayToCreateANewBookmarkList()
        {
            _favoritesPage.NewBookmarkListNameInput.Displayed.Should().BeTrue();
            _favoritesPage.CreateBookmarkListButton.Displayed.Should().BeTrue();
        }

        [Then("I should see the new bookmark list displayed")]
        public void ThenIShouldSeeTheNewBookmarkListDisplayed()
        {
            // Check that the new bookmark list is displayed and has title matching newBookmarkListTitle key
            _favoritesPage.BookmarkListTitles.Should().ContainSingle(e => e.Text == _scenarioContext["newBookmarkListTitle"].ToString());
        }

        [When("I fill out and submit the new bookmark list form with a unique title")]
        public void WhenIFillOutAndSubmitTheNewBookmarkListFormWithAUniqueTitle()
        {
            // Fill out the new bookmark list form with a unique title and put the title in the scenario context
            string newBookmarkListTitle = "New Bookmark List " + DateTime.Now.ToString("yyyyMMddHHmmss");
            _scenarioContext["newBookmarkListTitle"] = newBookmarkListTitle;

            _favoritesPage.NewBookmarkListNameInput.SendKeys(newBookmarkListTitle);
            _browserDriver.ScrollToElement(_favoritesPage.CreateBookmarkListButton);
            _favoritesPage.CreateBookmarkListButton.Click();
        }

        [When("I fill out the new bookmark list name input with an empty value")]
        public void WhenIFillOutTheNewBookmarkListNameInputWithAnEmptyValue()
        {
            _browserDriver.ScrollToElement(_favoritesPage.NewBookmarkListNameInput);
            // Confirm that the new bookmark list name input is empty
            _favoritesPage.NewBookmarkListNameInput.Clear();
        }

        [Then("I should see the create bookmark list button is disabled")]
        public void ThenIShouldSeeTheCreateBookmarkListButtonIsDisabled()
        {
            // Check that the create bookmark list button has disabled attribute
            _favoritesPage.CreateBookmarkListButton.Enabled.Should().BeFalse();
        }

        [Then("I should see the new bookmark list form is cleared")]
        public void ThenIShouldSeeTheNewBookmarkListFormIsCleared()
        {
            // Check that the new bookmark list name input is empty
            _favoritesPage.NewBookmarkListNameInput.Text.Should().BeEmpty();
        }

        [Then("The new bookmark list should have a default image")]
        public void ThenTheNewBookmarkListShouldHaveADefaultImage()
        {
            // Get the bookmark list from scenario context
            string newBookmarkListTitle = _scenarioContext["newBookmarkListTitle"].ToString();
            // Check that the new bookmark list has a default image as a background image
            var bookmarkList = _favoritesPage.GetBookmarkListFromTitle(newBookmarkListTitle);

            // check that the background-image css property is set to the default image
            bookmarkList.GetCssValue("background-image").Should().Contain("url(\"http://localhost:5145/media/images/placeholder_event_card_image.png\")");
        }
    }
}
