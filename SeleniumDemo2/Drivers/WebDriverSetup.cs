using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumDemo2.Drivers
{
    public class WebDriverSetup
    {
        private IWebDriver _driver = null!;

        public IWebDriver InitializeDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            _driver = new ChromeDriver(options);
            return _driver;
        }

        public void CloseDriver()
        {
            _driver.Quit();
        }
    }
}
