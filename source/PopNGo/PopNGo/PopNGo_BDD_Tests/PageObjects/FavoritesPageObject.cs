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
        public IReadOnlyList<IWebElement> BookmarkListTitles => _webDriver.FindElements(By.ClassName("bookmarkListCardTitleText"));
        public IWebElement CreateBookmarkListButton => _webDriver.FindElement(By.ClassName("saveNewBookmarkListButton"));
        public IWebElement NewBookmarkListNameInput => _webDriver.FindElement(By.Id("new-bookmark-list-card-title-input"));
    }
}
