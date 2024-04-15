using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;

namespace PopNGo_BDD_Tests.Drivers
{
    /// <summary>
    /// Manages a browser instance using Selenium.  From: https://docs.specflow.org/projects/specflow/en/latest/ui-automation/Selenium-with-Page-Object-Pattern.html
    /// An instance of this class is created and provided by the DI container
    /// </summary>
    public class BrowserDriver : IDisposable
    {
        private readonly Lazy<IWebDriver> _currentWebDriverLazy;
        private IJavaScriptExecutor js;
        private bool _isDisposed = false;

        public BrowserDriver()
        {
            _currentWebDriverLazy = new Lazy<IWebDriver>(CreateWebDriver);
            js = (IJavaScriptExecutor)Current;
        }

        /// <summary>
        /// The Selenium IWebDriver instance.  It is loaded when the .Value is invoked (otherwise lazy)
        /// </summary>
        public IWebDriver Current => _currentWebDriverLazy.Value;

        /// <summary>
        /// Creates the Selenium web driver (opens a browser)
        /// </summary>
        /// <returns></returns>
        private IWebDriver CreateWebDriver()
        {
            //Chrome browser
            //ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            //ChromeOptions chromeOptions = new ChromeOptions();
            //ChromeDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

            // Firefox (never trusts the self-signed cert when running locally, so must bypass)
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AcceptInsecureCertificates = true;

            // Enable location services
            firefoxOptions.SetPreference("geo.enabled", true);
            firefoxOptions.SetPreference("geo.provider.use_corelocation", true);
            firefoxOptions.SetPreference("geo.prompt.testing", true);
            firefoxOptions.SetPreference("geo.prompt.testing.allow", true);
            firefoxOptions.SetPreference("geo.provider.network.url", 
                "data:application/json,{\"location\": {\"lat\": 44.8485, \"lng\": -123.234}, \"accuracy\": 100.0}");

            FirefoxDriver driver = new FirefoxDriver(firefoxOptions);

            // Edge browser
            // EdgeDriverService edgeDriverService = EdgeDriverService.CreateDefaultService();
            // EdgeOptions edgeOptions = new EdgeOptions();
            // EdgeDriver driver = new EdgeDriver(edgeDriverService, edgeOptions);


            return driver;
        }

        /// <summary>
        /// Disposes the Selenium web driver (closing the browser) after the Scenario completed
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_currentWebDriverLazy.IsValueCreated)
            {
                Current.Quit();
            }

            _isDisposed = true;
        }

        public void ScrollToElement(IWebElement element)
        {
            if (element != null)
            {
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                Thread.Sleep(1000);
            }
            else
            {
                throw new ArgumentNullException("Element is null");
            }
        }
    }
}