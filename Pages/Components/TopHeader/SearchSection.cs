using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Ebay.Automation.Framework.Pages.Components.TopHeader
{
    public class SearchSection
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _root;
        private readonly WebDriverWait _wait;

        // Locators
        private readonly By inputSearch = By.Id("gh-ac");
        private readonly By selectCategory = By.Id("gh-cat");
        private readonly By searchButton = By.Id("gh-search-btn");

        public SearchSection(IWebElement headerRoot, IWebDriver driver)
        {
            _driver = driver;
            _root = headerRoot;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Enters text into the search input field.
        /// </summary>
        /// <param name="text">Text to enter</param>
        public void EnterSearchText(string text)
        {
            IWebElement inputElement = _wait.Until(d => 
            {
                var element = d.FindElement(inputSearch);
                return element.Displayed && element.Enabled ? element : null;
            });
            
            inputElement.Clear();
            inputElement.SendKeys(text);
        }

        /// <summary>
        /// Clicks the search button to perform the search.
        /// </summary>
        public void ClickSearchButton()
        {
            IWebElement searchBtn = _wait.Until(d => 
            {
                var element = d.FindElement(searchButton);
                return element.Displayed && element.Enabled ? element : null;
            });
            
            searchBtn.Click();
        }

        /// <summary>
        /// Selects a category from the dropdown by visible text.
        /// </summary>
        /// <param name="categoryName">Category name to select</param>
        public void SelectCategory(string categoryName)
        {
            IWebElement categoryElement = _wait.Until(d => 
            {
                var element = d.FindElement(selectCategory);
                return element.Displayed && element.Enabled ? element : null;
            });
            
            var selectElement = new SelectElement(categoryElement);
            selectElement.SelectByText(categoryName);
        }
    }
}
