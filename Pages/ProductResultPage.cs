using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ebay.Automation.Framework.Pages
{
    public class ProductResultPage : BasePage
    {
        // Locators
        private readonly By listingOptionsButton = By.CssSelector("button.fake-menu-button__button[aria-label*='Listing options selector']");
        private readonly By listingMenuItems = By.CssSelector("ul.fake-menu__items li a.fake-menu-button__item");
        private readonly By ResultsUlLocator = By.CssSelector("#srp-river-main ul.srp-results");
        private readonly By ProductLiLocator = By.CssSelector("li.s-card[data-listingid]");
        private readonly By productPriceLocator = By.CssSelector("span.s-card__price");
        private readonly By productTitleLocator = By.CssSelector("div.s-card__title span[class~='primary']");
        private readonly By productLinkLocator = By.CssSelector("a.s-card__link");
        

        public ProductResultPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Changes the listing view (List / Gallery).
        /// </summary>
        /// <param name="view">View name (e.g., "List", "Gallery")</param>
        public void ChangeListingView(string view)
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);

            // Click button to open menu
            var button = wait.Until(d =>
            {
                var el = d.FindElement(listingOptionsButton);
                return el.Displayed ? el : null;
            });
            button.Click();

            // Find and click the option by text
            var option = wait.Until(d =>
            {
                var items = d.FindElements(listingMenuItems);
                return items.FirstOrDefault(x => x.Text.Contains(view, StringComparison.OrdinalIgnoreCase));
            });

            if (option == null)
                throw new NoSuchElementException($"View option '{view}' not found!");

            option.Click();
        }

        public IWebElement GetProductByIndex(int index)
        {
            IWebElement ulContainer = Driver.FindElement(ResultsUlLocator);

            IList<IWebElement> products = ulContainer.FindElements(ProductLiLocator);
            return products[index];
        }

        public IWebElement GetProductPrice(int index)
        {
            IWebElement ulContainer = Driver.FindElement(ResultsUlLocator);

            IList<IWebElement> products = ulContainer.FindElements(ProductLiLocator);
            return products[index].FindElement(productPriceLocator);
        }

        public IWebElement GetProductTitle(int index)
        {
            IWebElement ulContainer = Driver.FindElement(ResultsUlLocator);

            IList<IWebElement> products = ulContainer.FindElements(ProductLiLocator);
            return products[index].FindElement(productTitleLocator);
        }

        public void ClickProductByIndex(int index)
        {
            IWebElement ulContainer = Driver.FindElement(ResultsUlLocator);
            IList<IWebElement> products = ulContainer.FindElements(ProductLiLocator);
            IWebElement productLink = products[index].FindElement(productLinkLocator);
            productLink.Click();
        }
    }
}
