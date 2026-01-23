using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;

namespace Ebay.Automation.Framework.Pages
{
    public class DetailedProductPage : BasePage
    {
        private readonly By mainContentLocator = By.Id("mainContent");
        private readonly By productTitleLocator = By.CssSelector("div[data-testid='x-item-title'] span");
        private readonly By productPriceLocator = By.CssSelector("div[data-testid='x-price-primary'] span");

        private readonly By returnsSectionLocator = By.CssSelector("div[data-testid='x-returns-minview']");
        private readonly By buttonSeeDetailsLocator = By.CssSelector("button[data-testid='ux-action']");
        private readonly By tabsRegionLocator = By.CssSelector("div[data-testid='x-evo-tabs-region']");
        private readonly By modalTabsLocator = By.CssSelector("div[role='tab'].tabs__item");
        private readonly By shipsToSectionLocator = By.CssSelector("div.ux-labels-values--shipsto");
        private readonly By paymentMethodsSectionLocator = By.CssSelector("div.ux-section--paymentmethods");
        private readonly By modalLocator = By.CssSelector("div.lightbox-dialog[role='dialog']:not([hidden])");
        private readonly By closeModalBtnLocator = By.CssSelector("button.lightbox-dialog__close");
        private readonly By quantityInputLocator = By.Id("qtyTextBox");
        private readonly By buyboxButtonsLocator = By.CssSelector("ul[data-testid='x-buybox-cta'] a.ux-call-to-action");
        private readonly By addToCartModalLocator = By.CssSelector("div.x-atc-layer-v3[data-testid='x-atc-layer-v3']");
        private readonly By seeInCartBtnLocator = By.CssSelector("a[data-testid='ux-call-to-action'].fake-btn--primary");


        private IWebElement MainContainer => Driver.FindElement(mainContentLocator);

        public DetailedProductPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetProductTitle()
        {
            return MainContainer.FindElement(productTitleLocator).Text;
        }

        public string GetProductPrice()
        {
            string rawPrice = MainContainer.FindElement(productPriceLocator).Text;
            return Regex.Replace(rawPrice, @"[A-Za-z\s]+", "");
        }

        public void SetQuantity(int quantity)
        {
            var qtyInput = MainContainer.FindElement(quantityInputLocator);
            qtyInput.Clear();
            qtyInput.SendKeys(quantity.ToString());
        }

        public void ClickSeeDetailsBtn()
        {
            var returnSection = MainContainer.FindElement(returnsSectionLocator);
            var seeDetailsBtn = returnSection.FindElement(buttonSeeDetailsLocator);
            seeDetailsBtn.Click();
            
            // Wait for modal to open
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            wait.Until(d => d.FindElement(modalLocator).Displayed);
        }

        public void ClickModalTab(string tabName)
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            var tabs = wait.Until(d => d.FindElements(modalTabsLocator));
            var targetTab = tabs.FirstOrDefault(t => t.Text.Contains(tabName, StringComparison.OrdinalIgnoreCase));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", targetTab ?? throw new Exception($"Tab '{tabName}' not found"));
        }

        public bool IsCountryInShipsList(string countryName)
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            var shipsToSection = wait.Until(d => d.FindElement(shipsToSectionLocator));
            try 
            { 
                shipsToSection.FindElement(By.CssSelector("details summary")).Click();
                // Wait for expansion animation
                wait.Until(d => shipsToSection.FindElements(By.CssSelector("span.ux-expandable-textual-display-block-inline")).Any(e => !e.GetAttribute("class").Contains("hide")));
            } 
            catch { }
            var visibleElement = shipsToSection.FindElements(By.CssSelector("span.ux-expandable-textual-display-block-inline"))
                .FirstOrDefault(e => !e.GetAttribute("class").Contains("hide"));
            var countriesText = visibleElement?.FindElement(By.CssSelector("span[data-testid='text'] span.ux-textspans")).Text ?? "";
            return countriesText.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                .Any(c => c.Equals(countryName, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsPaymentMethodAvailable(string paymentMethod)
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            var paymentSection = wait.Until(d => d.FindElement(paymentMethodsSectionLocator));
            var paymentSpans = paymentSection.FindElements(By.CssSelector("span[role='img']"));
            return paymentSpans.Any(s => 
                (s.GetAttribute("aria-label")?.Contains(paymentMethod, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (s.GetAttribute("title")?.Contains(paymentMethod, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        public void CloseModal()
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            var modal = wait.Until(d => d.FindElement(modalLocator));
            var closeBtn = modal.FindElement(closeModalBtnLocator);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", closeBtn);
            
            // Wait for modal to close
            wait.Until(d => 
            {
                try { return !d.FindElement(modalLocator).Displayed; }
                catch (NoSuchElementException) { return true; }
            });
        }

        public void ClickActionButton(string buttonText)
        {
            var buttons = MainContainer.FindElements(buyboxButtonsLocator);
            var targetBtn = buttons.FirstOrDefault(b => b.Text.Contains(buttonText, StringComparison.OrdinalIgnoreCase));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", targetBtn ?? throw new Exception($"Button '{buttonText}' not found"));
        }

        public void ClickSeeInCart()
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            var atcModal = wait.Until(d => d.FindElement(addToCartModalLocator));
            var seeInCartBtn = atcModal.FindElement(seeInCartBtnLocator);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", seeInCartBtn);
            
            // Wait for navigation to cart
            wait.Until(d => d.Url.Contains("cart.ebay.com"));
        }
    }
}