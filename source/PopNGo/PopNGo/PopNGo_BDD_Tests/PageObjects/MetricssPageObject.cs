using OpenQA.Selenium;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class MetricsPageObject : PageObject
    {
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