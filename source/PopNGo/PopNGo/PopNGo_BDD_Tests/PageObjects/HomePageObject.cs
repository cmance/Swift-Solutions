using Microsoft.Identity.Client;
using OpenQA.Selenium;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class HomePageObject : PageObject
    {
        public HomePageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Home";
        }
        public IWebElement NavigationBar => _webDriver.FindElement(By.Id("custom-bottom-navbar-id"));

        // Select the "Home" navigation item by id="home-logo"
        public IWebElement homeLogo => _webDriver.FindElement(By.Id("home-logo"));

        // Select the "Explore" navigation item by id="explore-logo"
        public IWebElement exploreNavItem => _webDriver.FindElement(By.Id("explore-logo"));

        // Select the "Favorites" navigation item by id="favorites-logo"
        public IWebElement favoritesNavItem => _webDriver.FindElement(By.Id("favorites-logo"));

        // Select the "History" navigation item by id="history-logo"
        public IWebElement historyNavItem => _webDriver.FindElement(By.Id("history-logo"));

        public IWebElement hideNavigationBar => _webDriver.FindElement(By.Id("close-navbar"));

        public IWebElement showNavigationBar => _webDriver.FindElement(By.Id("close-navbar-2"));

        public IWebElement hiddenNavigationBar => _webDriver.FindElement(By.ClassName("minimized-navbar"));

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

        public void CloseNavigationBar()
        {
            hideNavigationBar.Click();
        }

        public void OpenNavigationBar()
        {
            showNavigationBar.Click();
        }

    }
}