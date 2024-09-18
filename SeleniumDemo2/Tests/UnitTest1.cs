using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace SeleniumDemo2.Tests
{
    [TestFixture]
    public class SeleniumTests
    {
        
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
        }

        [Test]
        public void GoogleSearchTest()
        {
            _driver.Navigate().GoToUrl("https://www.booking.com");

            // Locate the login button by class value
            IWebElement loginButton = _driver.FindElement(By.CssSelector(".e4adce92df"));
            loginButton.Click();
            //loginPage

            // Locate the element by id
            //string element = _driver.FindElement(By.CssSelector(".osvS4MYxeSR4s9RPRMlw nw-step-header")).Text;

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".osvS4MYxeSR4s9RPRMlw")));
            string text = element.Text;

            // Assert that the text is "Sign in or create an account"
            Assert.That(text, Is.EqualTo("Sign in or create an account"), "The button text is not as expected.");
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}