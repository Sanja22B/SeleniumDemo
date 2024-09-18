using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumDemo2.Pages
{
    public class SearchPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Locators
        private readonly By _searchButtonLocator = By.XPath("//button[@type='submit']");
        private readonly By _yourAccountButton = By.XPath("//span[contains(text(), 'Your account')]");
        private readonly By _destionationInputLocator = By.CssSelector(".eb46370fe1");
        private readonly By _personPickerLocator = By.CssSelector("[data-testid='searchbox-form-button-icon']");
        private readonly By _adultsLocatorIncreaser = By.CssSelector(".eedba9e88a");
        private readonly By _childrenLocatorIncreaser = By.CssSelector(".eedba9e88a");
        private readonly By _roomsLocatorIncreaser = By.CssSelector(".eedba9e88a");
        private readonly By _loadMoreResultsButton = By.XPath("//span[contains(text(), 'Load More Results')]");
        private readonly By _signOutButton = By.XPath("//span[contains(text(), 'Sign Out')]");
        private readonly By _loginButtonLocator = By.XPath("//span[contains(text(), 'Sign in')]");
        private readonly By _datePicker = By.ClassName(".e22b782521");
        private readonly By _MonthYearLocator = By.ClassName(".e1eebb6a1e ee7ec6b631");
        private readonly By _nextMonthLocator = By.ClassName(".eedba9e88a");
        
        
        // Constructor to initialize the page and validate that Log in was successful
        public SearchPage(IWebDriver driver)
        {
            this._driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement yourAccountButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_yourAccountButton));

            if (!yourAccountButton.Displayed)
            {
                Assert.IsTrue(yourAccountButton.Displayed, "This is not the Search Page");
            }
        }

        public void EnterLocationQuery(string query)
        {
            IWebElement yourAccountButton =
                _wait.Until(ExpectedConditions.ElementToBeClickable(_destionationInputLocator));
            yourAccountButton.SendKeys(query);
        }

        public void SubmitSearch()
        {
            _driver.FindElement(_searchButtonLocator).Submit();
        }

        public void EnterCheckInCheckOut(string day, string month, string year)
        {
            // Find and click the date picker input to open the calendar
            IWebElement datePicker = _wait.Until(ExpectedConditions.ElementToBeClickable(_datePicker));
            datePicker.Click();
            IWebElement monthYearSelector = _wait.Until(ExpectedConditions.ElementToBeClickable(_MonthYearLocator));
            var monthYear = monthYearSelector.Text;
            var monthCalendar = monthYear.Split(" ")[0].Trim();
            var yearCalendar  = monthYear.Split(" ")[1].Trim();
            
            while(!(month.Equals(month) && year.Equals(year)))
            {
                IWebElement nextMonth = _wait.Until(ExpectedConditions.ElementToBeClickable(_nextMonthLocator));
                datePicker.Click();
            }
            

        }

        public void EnterPersonPicker()
        {
            IWebElement personPicker = _wait.Until(ExpectedConditions.ElementToBeClickable(_personPickerLocator));
            personPicker.Click();
            //increase number of adults
            IWebElement adultsIncrease = _wait.Until(ExpectedConditions.ElementToBeClickable(_adultsLocatorIncreaser));
            adultsIncrease.Click();
            //increase children
            IWebElement childrenIncrease =
                _wait.Until(ExpectedConditions.ElementToBeClickable(_childrenLocatorIncreaser));
            childrenIncrease.Click();
            //increase rooms
            IWebElement roomIncrease = _wait.Until(ExpectedConditions.ElementToBeClickable(_roomsLocatorIncreaser));
            roomIncrease.Click();
        }

        public void LoadMoreResults()
        {
            IWebElement loadMoreResults = null;
            try
            {
                while (true)
                {
                    try
                    {
                        loadMoreResults = _wait.Until(ExpectedConditions.ElementToBeClickable(_loadMoreResultsButton));
                        if (loadMoreResults.Displayed)
                        {
                            break;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        // Scroll down if the button is not found
                        ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, 1000);");
                        Thread.Sleep(1000);
                    }
                }

                loadMoreResults.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

        }

        public void Signout()
        {
            IWebElement yourAccount = _wait.Until(ExpectedConditions.ElementToBeClickable(_yourAccountButton));
            yourAccount.Click();
            IWebElement signOut = _wait.Until(ExpectedConditions.ElementToBeClickable(_signOutButton));
            signOut.Click();
            //assert that "sign in" button is visible
            IWebElement loginButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_loginButtonLocator));
            Assert.IsFalse(loginButton.Displayed, "The login button is not visible.");

        }
    }
}
