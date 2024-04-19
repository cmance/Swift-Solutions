using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class AdminPageObject : PageObject
    {
        public AdminPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Admin";
        }
        public IWebElement NavigationBar => _webDriver.FindElement(By.ClassName("navbar"));
        public IWebElement LoginButton => _webDriver.FindElement(By.Id("login-button"));
        public IWebElement AdminButton => _webDriver.FindElement(By.Id("admin-section-access"));

        // Select the "Notifications" navigation item by id="scheduled-notifications"
        public IWebElement scheduledNotificationsNavItem => _webDriver.FindElement(By.Id("scheduled-notifications"));
    }
}