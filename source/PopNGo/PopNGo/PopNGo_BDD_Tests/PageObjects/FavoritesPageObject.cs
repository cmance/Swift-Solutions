using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class FavoritesPageObject : PageObject
    {
        public FavoritesPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Favorites";
        }

        public IReadOnlyList<IWebElement> BookmarkLists => _webDriver.FindElements(By.ClassName("bookmarkListCard"));
        public IWebElement GetBookmarkListFromTitle(string title) {
            return BookmarkLists.FirstOrDefault(e => e.FindElement(By.ClassName("bookmarkListCardTitleText")).Text == title);
        }
        public IWebElement GetDeleteButtonFromBookmarkList(string title) {
            return GetBookmarkListFromTitle(title).FindElement(By.ClassName("bookmarkListCardDeleteButton"));
        }
        public IWebElement GetEditButtonFromBookmarkList(string title) {
            return GetBookmarkListFromTitle(title).FindElement(By.ClassName("bookmarkListCardEditButton"));
        }
        public IWebElement DeleteBookmarkListConfirmationModal => _webDriver.FindElement(By.Id("delete-bookmark-list-confirmation-modal"));
        public IWebElement EditBookmarkListModal => _webDriver.FindElement(By.Id("edit-bookmark-list-modal"));
        public IWebElement CancelEditBookmarkListButton => _webDriver.FindElement(By.Id("cancel-edit-bookmark-list-button"));
        public IWebElement EditBookmarkListModalSaveButton => _webDriver.FindElement(By.Id("save-edit-bookmark-list-button"));
        public IWebElement EditBookmarkListNameInput => _webDriver.FindElement(By.Id("edit-bookmark-list-modal-title-input"));
        public IReadOnlyList<IWebElement> BookmarkListTitles => _webDriver.FindElements(By.ClassName("bookmarkListCardTitleText"));
        public IWebElement CreateBookmarkListButton => _webDriver.FindElement(By.ClassName("saveNewBookmarkListButton"));
        public IWebElement NewBookmarkListNameInput => _webDriver.FindElement(By.Id("new-bookmark-list-card-title-input"));
    }
}
