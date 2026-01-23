using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Ebay.Automation.Framework.Pages.Components.TopHeader;

namespace Ebay.Automation.Framework.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver { get; }
        protected static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

        private TopHeaderComponent? _header;
        
        // Global header component available on all pages
        public TopHeaderComponent Header 
        { 
            get 
            {
                _header ??= new TopHeaderComponent(Driver);
                return _header;
            }
        }

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        /// <summary>
        /// Navigates to a specified URL and waits for page to fully load.
        /// </summary>
        /// <param name="url">Target page URL</param>
        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
            WaitForPageLoad();
        }

        /// <summary>
        /// Waits for the page to be fully loaded (document.readyState == 'complete').
        /// </summary>
        public void WaitForPageLoad()
        {
            var wait = new WebDriverWait(Driver, DefaultTimeout);
            wait.Until(d =>
                ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete"
            );
        }

        /// <summary>
        /// Switches to the last opened window/tab.
        /// </summary>
        public void SwitchToLastWindow()
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
        }

        /// <summary>
        /// Checks if the current URL contains the specified text.
        /// </summary>
        /// <param name="urlPart">Text to search for in the current URL</param>
        /// <returns>True if the URL contains the specified text, otherwise false</returns>
        public bool IsUrlContaining(string urlPart)
        {
            string currentUrl = Driver.Url;
            return currentUrl.Contains(urlPart, StringComparison.OrdinalIgnoreCase);
        }
    }
}
