using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V142.Network;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Ebay.Automation.Framework.Pages
{
    public class CartPage : BasePage
    {

        private readonly By appCartLocator = By.CssSelector("div[data-test-id='app-cart']");
        private readonly By productQuantityLocator = By.CssSelector("div.quantity input");
        private readonly By productSummaryLocator = By.CssSelector("div[data-test-id='cart-summary']");

        private readonly By productTotalSumLocator = By.CssSelector("div[data-test-id='ITEM_TOTAL']");

        private IWebElement CartContainer
        {
            get
            {
                var wait = new WebDriverWait(Driver, DefaultTimeout);
                return wait.Until(d =>
                {
                    try
                    {
                        var element = d.FindElement(appCartLocator);
                        return element.Displayed ? element : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
            }
        }

        public CartPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetProductQuantity()
        {
            return CartContainer.FindElement(productQuantityLocator).GetAttribute("value");
        }

        public string GetProductTotalSum()
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            var summary = wait.Until(d => d.FindElement(productSummaryLocator));
            return summary.FindElement(productTotalSumLocator).Text;
        }
    }
}