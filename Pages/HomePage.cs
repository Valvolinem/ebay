using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ebay.Automation.Framework.Pages
{
    public class HomePage : BasePage
    {
        // Locators
        private readonly By ebayLogoTitle = By.Id("ebayLogoTitle");

        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Gets the eBay logo title text.
        /// </summary>
        /// <returns>Logo title text</returns>
        public string GetLogoTitle()
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            var element = wait.Until(d => d.FindElement(ebayLogoTitle));
            return element.Text;
        }
    }
}
