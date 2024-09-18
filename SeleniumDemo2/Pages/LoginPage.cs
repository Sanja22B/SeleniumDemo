using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;



namespace SeleniumDemo2.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Locators
        private readonly By _loginButtonLocator = By.XPath("//span[contains(text(), 'Sign in')]");
        private readonly By _emailInputLocator = By.CssSelector("input[name='username'][type='email']");
        private readonly By _continueButtonLocator = By.XPath("//button[@type='submit']");
        private readonly By _passwordInputLocator = By.XPath("//input[@name='password' and @type='password']");
        private readonly By _submitButtonLocator = By.XPath("//button[@type='submit']");
        private readonly By _modalLocator=By.CssSelector(".c0528ecc22");
        private readonly By _modalCloseButtonLocator=By.CssSelector(".eedba9e88a");
        private readonly By _modalSecurityCheckButtonLocator = By.XPath("//span[contains(text(), 'Press and hold this button to pass the security check')]");
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        
        public void HandleModalIfPresent(IWebDriver driver)
        {
            try
            {
                
                IWebElement modal = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_modalLocator));

                // If modal appears
                IWebElement closeButton = modal.FindElement(_modalCloseButtonLocator);
                closeButton.Click();
                _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_modalLocator));
                // Assert that the modal is no longer displayed
                //Assert.IsFalse(_modalLocator, "The modal is still visible after clicking on the page.");

                Console.WriteLine("Modal appeared and was handled.");
            }
            catch (WebDriverTimeoutException)
            {
                // Modal didn't appear within the timeout, continue without any action
                Console.WriteLine("Modal did not appear.");
            }
        }

        // Methods for interacting with the page
        public void NavigateToLoginPage()
        {
            _driver.Navigate().GoToUrl("https://www.booking.com");
            HandleModalIfPresent(_driver);
            IWebElement loginButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_loginButtonLocator));
            loginButton.Click();
        }
        
        // Wait until an element is clickable, then return it
        private IWebElement WaitUntilElementClickable(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public void EnterEmail(string email)
        {
            IWebElement emailInput = WaitUntilElementClickable(_emailInputLocator);
            emailInput.SendKeys(email);

            IWebElement continueButton = WaitUntilElementClickable(_continueButtonLocator);
            continueButton.Click();
        }

        public void EnterPassword(string password)
        {
            IWebElement passwordInput = WaitUntilElementClickable(_passwordInputLocator);
            passwordInput.SendKeys(password);
        }
        
        public SearchPage ClickLoginButton()
        {
            IWebElement submitButton = WaitUntilElementClickable(_submitButtonLocator);
            submitButton.Click();
            //security check 
            IWebElement pressAndHoldButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(_modalSecurityCheckButtonLocator));
            Assert.IsTrue(pressAndHoldButton.Displayed);
            if (pressAndHoldButton.Displayed)
            {
                Console.WriteLine("Security check button.");
                Actions actions = new Actions(_driver);
                //IWebElement pressAndHoldButton = WaitUntilElementClickable(_modalSecurityCheckButtonLocator);
                actions.ClickAndHold(pressAndHoldButton).Perform();
                // Hold for a specific amount of time (e.g., 5 seconds)
                Thread.Sleep(5000);
                // Release the button
                actions.Release().Perform();
            }
            return new SearchPage(_driver);
        }
    }
}
