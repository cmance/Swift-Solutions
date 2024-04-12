using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using System.Security.Permissions;
>>>>>>> 38cbe6f2e35377718afec52d456aebcf81f5045e

namespace PopNGo_BDD_Tests.Drivers
{
    /// <summary>
    /// Manages a browser instance using Selenium.  From: https://docs.specflow.org/projects/specflow/en/latest/ui-automation/Selenium-with-Page-Object-Pattern.html
    /// An instance of this class is created and provided by the DI container
    /// </summary>
    public class BrowserDriver : IDisposable
    {
        private readonly Lazy<IWebDriver> _currentWebDriverLazy;
<<<<<<< HEAD
=======
        private IJavaScriptExecutor js;
>>>>>>> 38cbe6f2e35377718afec52d456aebcf81f5045e
        private bool _isDisposed = false;

        public BrowserDriver()
        {
            _currentWebDriverLazy = new Lazy<IWebDriver>(CreateWebDriver);
<<<<<<< HEAD
=======
            js = (IJavaScriptExecutor)Current;
>>>>>>> 38cbe6f2e35377718afec52d456aebcf81f5045e
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
<<<<<<< HEAD
            // Firefox (never trusts the self-signed cert when running locally, so must bypass)
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AcceptInsecureCertificates = true;

            // Enable location services
            firefoxOptions.SetPreference("geo.enabled", true);
            firefoxOptions.SetPreference("geo.provider.use_corelocation", true);
            firefoxOptions.SetPreference("geo.prompt.testing", true);
            firefoxOptions.SetPreference("geo.prompt.testing.allow", true);

            FirefoxDriver driver = new FirefoxDriver(firefoxOptions);

=======
            //Chrome browser
            //ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            //ChromeOptions chromeOptions = new ChromeOptions();
            //ChromeDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

            // Firefox (never trusts the self-signed cert when running locally, so must bypass)
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.AcceptInsecureCertificates = true;
            FirefoxDriver driver = new FirefoxDriver(firefoxOptions);

            // Edge browser
            // EdgeDriverService edgeDriverService = EdgeDriverService.CreateDefaultService();
            // EdgeOptions edgeOptions = new EdgeOptions();
            // EdgeDriver driver = new EdgeDriver(edgeDriverService, edgeOptions);


>>>>>>> 38cbe6f2e35377718afec52d456aebcf81f5045e
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
<<<<<<< HEAD
=======

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
>>>>>>> 38cbe6f2e35377718afec52d456aebcf81f5045e
    }
}