using OpenQA.Selenium;
using FluentAssertions.Extensions;

namespace PopNGo_BDD_Tests.PageObjects
{
    public class RecommendationPageObject : PageObject
    {
        public RecommendationPageObject(IWebDriver webDriver) : base(webDriver)
        {
            // using a named page (in Common.cs)
            _pageName = "Recommendation";
        }
        






    }
}