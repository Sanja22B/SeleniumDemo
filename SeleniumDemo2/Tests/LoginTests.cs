
using SeleniumDemo2.Drivers;
using SeleniumDemo2.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumDemo2.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver _driver;
        private WebDriverSetup _webDriverSetup;
        private LoginPage _loginPage;
        private readonly WebDriverWait _wait;
        //locators for assertions
        private readonly By _searchHeaderLocator=By.CssSelector(".f6431b446c");
        private readonly By _searchResultLocator=By.CssSelector(".dcf496a7b9");
        private readonly By _messageLocator = By.XPath("//div[contains(text(), 'The email and password combination you entered doesn't match')]");

        [SetUp]
        public void SetUp()
        {
            _webDriverSetup = new WebDriverSetup();
            _driver = _webDriverSetup.InitializeDriver();
            _loginPage = new LoginPage(_driver);
        }

        [Test]
        public void Login_With_Valid_Credentials()
        {
            
            string email = "sanya.maile.8888@gmail.com";
            string password = "SeleniumDemo1234";
            string queryDestination = "Korcula Island";
            
            string emailIncorrect = "incorrectemail@gmail.com";
            string passwordIncorrect = "Test1234";
            

            // Test Flow 1: Positive Login flow
            _loginPage.NavigateToLoginPage();
            _loginPage.EnterEmail(email);
            _loginPage.EnterPassword(password);
            
            var searchPage = _loginPage.ClickLoginButton();
            // Test Flow 2: Search flow - Location/Destination
            searchPage.EnterLocationQuery(queryDestination);
            searchPage.SubmitSearch();
            //assert that search is executed for the query destination
            IWebElement element = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_searchHeaderLocator));
            Assert.That(element.Text, Does.StartWith("Korcula Island: "), "Text does not start with 'Korcula Island: '");
            //                          - Check-In Date/ Check-out Date
            
            //                          - persons picker (adults, children, rooms)
            searchPage.EnterPersonPicker();
            searchPage.SubmitSearch();
            //assert that search result is still visible
            IWebElement searchResultList = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_searchResultLocator));
            Assert.IsTrue(searchResultList.Displayed,"Search result is visible");
            // Test Flow 3: Search flow - Location/Destination with pagination (Load more results)
            searchPage.LoadMoreResults();
            // Sing out and test for negative login flow
            searchPage.Signout();
            _loginPage.NavigateToLoginPage();
            _loginPage.EnterEmail(emailIncorrect);
            _loginPage.EnterPassword(passwordIncorrect);
            _loginPage.ClickLoginButton();
            //assert message
            IWebElement message = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_messageLocator));
            Assert.Equals(message.Text, "The email and password combination you entered doesn't match");
        }

        [TearDown]
        public void TearDown()
        {
            _webDriverSetup.CloseDriver();
            _driver.Dispose();
        }
    }
}
