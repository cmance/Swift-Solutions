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

        [Then("I should see a delete button for the new bookmark list")]
        public void ThenIShouldSeeADeleteButtonForTheNewBookmarkList()
        {
            // Get the bookmark list from scenario context
            string newBookmarkListTitle = _scenarioContext["newBookmarkListTitle"].ToString();
            // Check that the new bookmark list has a delete button
            _favoritesPage.GetDeleteButtonFromBookmarkList(newBookmarkListTitle).Displayed.Should().BeTrue();
        }

        [When("I click the delete button for the new bookmark list")]
        public void WhenIClickTheDeleteButtonForTheNewBookmarkList()
        {
            // Get the bookmark list from scenario context
            string newBookmarkListTitle = _scenarioContext["newBookmarkListTitle"].ToString();
            // Click the delete button for the new bookmark list
            _favoritesPage.GetDeleteButtonFromBookmarkList(newBookmarkListTitle).Click();
        }

        [Then("I should see a delete bookmark list confirmation dialog")]
        public void ThenIShouldSeeADeleteBookmarkListConfirmationDialog()
        {
            // Check that the delete bookmark list confirmation dialog is displayed
            _favoritesPage.DeleteBookmarkListConfirmationModal.Displayed.Should().BeTrue();
        }

        [Given("I have created a new bookmark list"), When("I have created a new bookmark list")]
        public void GivenIHaveCreatedANewBookmarkList()
        {
            // Fill out the new bookmark list form with a unique title and put the title in the scenario context
            string newBookmarkListTitle = "New Bookmark List " + DateTime.Now.ToString("yyyyMMddHHmmss");
            _scenarioContext["newBookmarkListTitle"] = newBookmarkListTitle;
            if (!_scenarioContext.ContainsKey("oldBookmarkListTitle"))
            {
                _scenarioContext["oldBookmarkListTitle"] = newBookmarkListTitle;
            }

            _favoritesPage.NewBookmarkListNameInput.SendKeys(newBookmarkListTitle);
            _browserDriver.ScrollToElement(_favoritesPage.CreateBookmarkListButton);
            _favoritesPage.CreateBookmarkListButton.Click();
        }

        [Then("I should see the new bookmark list")]
        public void ThenIShouldSeeTheNewBookmarkList()
        {
            // Check that the new bookmark list is displayed and has title matching newBookmarkListTitle key
            _favoritesPage.BookmarkListTitles.Should().ContainSingle(e => e.Text == _scenarioContext["newBookmarkListTitle"].ToString());
        }

        [Then("I should not see the bookmark list confirmation dialog")]
        public void ThenIShouldNotSeeTheBookmarkListConfirmationDialog()
        {
            // Check that the delete bookmark list confirmation dialog is not displayed
            _favoritesPage.DeleteBookmarkListConfirmationModal.Displayed.Should().BeFalse();
        }

        [When("I click the cancel button in the delete bookmark list confirmation dialog")]
        public void WhenIClickTheCancelButtonInTheDeleteBookmarkListConfirmationDialog()
        {
            // Click the cancel button in the delete bookmark list confirmation dialog
            _favoritesPage.DeleteBookmarkListConfirmationModal.FindElement(By.Id("cancel-delete-bookmark-list-confirmation-button")).Click();
        }

        [Given("I have clicked the delete button for the new bookmark list")]
        public void GivenIHaveClickedTheDeleteButtonForTheNewBookmarkList()
        {
            // Get the bookmark list from scenario context
            string newBookmarkListTitle = _scenarioContext["newBookmarkListTitle"].ToString();
            // Click the delete button for the new bookmark list
            _favoritesPage.GetDeleteButtonFromBookmarkList(newBookmarkListTitle).Click();
        }

        [Then("I should not see the delete bookmark list confirmation dialog")]
        public void ThenIShouldNotSeeTheDeleteBookmarkListConfirmationDialog()
        {
            // Check that the delete bookmark list confirmation dialog is not displayed
            _favoritesPage.DeleteBookmarkListConfirmationModal.Displayed.Should().BeFalse();
        }

        [Then("I should not see the new bookmark list")]
        public void ThenIShouldNotSeeTheNewBookmarkList()
        {
            // Check that the new bookmark list is not displayed
            _favoritesPage.BookmarkListTitles.Should().NotContain(e => e.Text == _scenarioContext["newBookmarkListTitle"].ToString());
        }

        [When("I click the confirm button in the delete bookmark list confirmation dialog")]
        public void WhenIClickTheConfirmButtonInTheDeleteBookmarkListConfirmationDialog()
        {
            // Click the confirm button in the delete bookmark list confirmation dialog
            _favoritesPage.DeleteBookmarkListConfirmationModal.FindElement(By.Id("delete-bookmark-list-confirmation-button")).Click();
        }

        [Then("I should see a button to edit the bookmark list")]
        public void ThenIShouldSeeAButtonToEditTheBookmarkList()
        {
            // Get the bookmark list from scenario context
            string newBookmarkListTitle = _scenarioContext["newBookmarkListTitle"].ToString();
            // Check that the new bookmark list has an edit button
            _favoritesPage.GetEditButtonFromBookmarkList(newBookmarkListTitle).Displayed.Should().BeTrue();
        }

        [When("I click the edit button")]
        public void WhenIClickTheEditButton()
        {
            // Get the bookmark list from scenario context
            string newBookmarkListTitle = _scenarioContext["newBookmarkListTitle"].ToString();
            // Click the edit button for the new bookmark list
            _favoritesPage.GetEditButtonFromBookmarkList(newBookmarkListTitle).Click();
        }

        [When("I click the edit button on the new bookmark list"), Given("I click the edit button on the new bookmark list")]
        public void WhenIClickTheEditButtonOnTheNewBookmarkList()
        {
            // Get the bookmark list from scenario context
            string newBookmarkListTitle = _scenarioContext["newBookmarkListTitle"].ToString();
            // Click the edit button for the new bookmark list
            _favoritesPage.GetEditButtonFromBookmarkList(newBookmarkListTitle).Click();
        }

        [Then("I should see a form to edit the bookmark list name")]
        public void ThenIShouldSeeAFormToEditTheBookmarkListName()
        {
            // Check that the edit bookmark list modal is displayed
            _favoritesPage.EditBookmarkListModal.Displayed.Should().BeTrue();
            // Check that the new bookmark list name input is displayed
            _favoritesPage.EditBookmarkListNameInput.Displayed.Should().BeTrue();
        }

        [When("I fill in the form with a new name")]
        public void WhenIFillInTheFormWithANewName()
        {
            // Fill in the form with a new name
            string newBookmarkListTitle = "New Bookmark List " + DateTime.Now.ToString("yyyyMMddHHmmss");
            _scenarioContext["oldBookmarkListTitle"] = _scenarioContext["newBookmarkListTitle"];
            _scenarioContext["newBookmarkListTitle"] = newBookmarkListTitle;

            _favoritesPage.EditBookmarkListNameInput.Clear();
            _favoritesPage.EditBookmarkListNameInput.SendKeys(newBookmarkListTitle);
        }

        [When("I click the save button in the edit bookmark list form")]
        public void WhenIClickTheSaveButtonInTheEditBookmarkListForm()
        {
            // Click the save button in the edit bookmark list form
            _favoritesPage.EditBookmarkListModalSaveButton.Click();
        }

        [Then("I should see the new name on the bookmark list")]
        public void ThenIShouldSeeTheNewNameOnTheBookmarkList()
        {
            // Check that the new bookmark list is displayed and has title matching newBookmarkListTitle key
            _favoritesPage.BookmarkListTitles.Should().ContainSingle(e => e.Text == _scenarioContext["newBookmarkListTitle"].ToString());

            // Check that the old bookmark list is not displayed
            _favoritesPage.BookmarkListTitles.Should().NotContain(e => e.Text == _scenarioContext["oldBookmarkListTitle"].ToString());
        }

        [When("I click the cancel button in the edit bookmark list form")]
        public void WhenIClickTheCancelButtonInTheEditBookmarkListForm()
        {
            // Click the cancel button in the edit bookmark list form
            _favoritesPage.CancelEditBookmarkListButton.Click();
        }

        [Then("I should see the original name on the bookmark list")]
        public void ThenIShouldSeeTheOriginalNameOnTheBookmarkList()
        {
            // Check that the old bookmark list is displayed and has title matching oldBookmarkListTitle key
            _favoritesPage.BookmarkListTitles.Should().ContainSingle(e => e.Text == _scenarioContext["oldBookmarkListTitle"].ToString());
        }

        [Then("I should not see the edit bookmark list form")]
        public void ThenIShouldNotSeeTheEditBookmarkListForm()
        {
            // Check that the edit bookmark list modal is not displayed
            _favoritesPage.EditBookmarkListModal.Displayed.Should().BeFalse();
        }

        [When("I fill in the update bookmark list name form with a blank name")]
        public void WhenIFillInTheUpdateBookmarkListNameFormWithABlankName()
        {
            // Fill in the form with a blank name
            _favoritesPage.EditBookmarkListNameInput.Clear();

            // Confirm that the new bookmark list name input is empty
            _favoritesPage.EditBookmarkListNameInput.Text.Should().BeEmpty();
        }

        [Then("The save edit button should be disabled")]
        public void ThenTheSaveEditButtonShouldBeDisabled()
        {
            // Check that the save edit button is disabled
            _favoritesPage.EditBookmarkListModalSaveButton.Enabled.Should().BeFalse();
        }

        [When("I fill in the update bookmark list name form with the same name")]
        public void WhenIFillInTheUpdateBookmarkListNameFormWithTheSameName()
        {
            // Fill in the form with the same name
            _favoritesPage.EditBookmarkListNameInput.Clear();
            _favoritesPage.EditBookmarkListNameInput.SendKeys(_scenarioContext["newBookmarkListTitle"].ToString());
        }
    }
}
