using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PopNGo_BDD_Tests.Shared;
using System;
using System.Collections.ObjectModel;
using PopNGo_BDD_Tests.PageObjects;
using FluentAssertions.Extensions;
using OpenQA.Selenium.Support.Extensions;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class MetricsPageObject : PageObject
    {
        string _scheduleId = "";
        public MetricsPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Metrics";
        }
        
        public IWebElement EmailActivityChart => _webDriver.FindElement(By.Id("email-activity-metrics-chart"));
        public IWebElement UserAccountChart => _webDriver.FindElement(By.Id("user-account-metrics-chart"));
        public IWebElement EventActivityChart => _webDriver.FindElement(By.Id("event-activity-metrics-chart"));
    }
}