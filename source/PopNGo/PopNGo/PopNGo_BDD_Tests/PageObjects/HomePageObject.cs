using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class HomePageObject : PageObject
    {
        public HomePageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Home";
        }
        public IWebElement NavigationBar => _webDriver.FindElement(By.ClassName("navbar"));

        // Select the "Home" navigation item by id="home-logo"
        public IWebElement homeLogo => _webDriver.FindElement(By.Id("home-logo"));

        // Select the "Explore" navigation item by id="explore-logo"
        public IWebElement exploreNavItem => _webDriver.FindElement(By.Id("explore-logo"));

        // Select the "Favorites" navigation item by id="favorites-logo"
        public IWebElement favoritesNavItem => _webDriver.FindElement(By.Id("favorites-logo"));

        // Select the "History" navigation item by id="history-logo"
        public IWebElement historyNavItem => _webDriver.FindElement(By.Id("history-logo"));

        public void ClickHomeLogo()
        {
            homeLogo.Click();
        }

        public void ClickExploreLogo()
        {
            exploreNavItem.Click();
        }

        public string GetHomeLogoColor()
        {
            return homeLogo.GetCssValue("color");
        }

    }
}