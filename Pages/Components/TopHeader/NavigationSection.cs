using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ebay.Automation.Framework.Pages.Components.TopHeader
{
    public class NavigationSection
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _root;
        private readonly IWebElement _navElement;
        private readonly WebDriverWait _wait;

        // Locators
        private readonly By signInLink = By.CssSelector("a[href*='signin.ebay.com']");
        private readonly By shipToButton = By.CssSelector("button.gh-ship-to__menu");
        private readonly By countryDropdownButton = By.CssSelector(".shipto__country-list .menu-button__button");
        private readonly By dropdownOptions = By.CssSelector(".menu-button__items .menu-button__item");
        private readonly By doneButton = By.CssSelector("button.shipto__close-btn");

        public NavigationSection(IWebElement headerRoot, IWebDriver driver)
        {
            _driver = driver;
            _root = headerRoot;
            _navElement = _root.FindElement(By.CssSelector("nav.gh-nav"));
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Clicks the Sign In link.
        /// </summary>
        public void ClickSignIn()
        {
            _navElement.FindElement(signInLink).Click();
        }

        public void ChangeShippingCountry(string countryName)
        {
            var shipBtn = _navElement.FindElement(shipToButton);
            shipBtn.Click();
            
            // Wait for modal to open with more flexible check
            _wait.Until(d => 
            {
                try
                {
                    var dropdown = d.FindElement(countryDropdownButton);
                    return dropdown.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
            
            SelectShippingCountry(countryName);
            
            // Wait for country to be selected and UI to update
            _wait.Until(d => 
            {
                var btn = d.FindElement(shipToButton);
                return btn.GetAttribute("aria-label")?.Contains(countryName, StringComparison.OrdinalIgnoreCase) ?? false;
            });
            
            // Find and click Done button from the driver, not from navElement
            _wait.Until(d => 
            {
                try
                {
                    var btn = d.FindElement(doneButton);
                    if (btn.Displayed && btn.Enabled)
                    {
                        btn.Click();
                        return true;
                    }
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
            
            // Wait for modal to close
            _wait.Until(d => 
            {
                try 
                { 
                    var btn = d.FindElement(doneButton);
                    return !btn.Displayed; 
                }
                catch (NoSuchElementException) { return true; }
                catch (StaleElementReferenceException) { return true; }
            });
        }

        public void SelectShippingCountry(string countryName)
        {
            IWebElement dropdownBtn = _wait.Until(d => 
            {
                var element = d.FindElement(countryDropdownButton);
                return element.Displayed && element.Enabled ? element : null;
            });
            
            dropdownBtn.Click();

            _wait.Until(d => d.FindElements(dropdownOptions).Count > 0);

            var options = _driver.FindElements(dropdownOptions);

            IWebElement targetCountry = options.FirstOrDefault(opt =>
                opt.Text.Contains(countryName, StringComparison.OrdinalIgnoreCase));

            if (targetCountry != null)
            {
                // Scroll to element
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", targetCountry);
                
                // Wait for element to be ready after scroll
                _wait.Until(d => targetCountry.Displayed && targetCountry.Enabled);
                
                // Always use JavaScript click for reliability
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", targetCountry);
                
                // Wait for country selection to be reflected in the ship button
                var shipBtnUpdated = _wait.Until(d => 
                {
                    try
                    {
                        var btn = d.FindElement(shipToButton);
                        var label = btn.GetAttribute("aria-label");
                        return !string.IsNullOrEmpty(label) && label.Contains(countryName, StringComparison.OrdinalIgnoreCase);
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                });
                
                if (!shipBtnUpdated)
                {
                    throw new Exception($"Country '{countryName}' was not reflected in the ship button");
                }
            }
            else
            {
                throw new Exception($"Country '{countryName}' not found in the dropdown");
            }
        }
    }
}
