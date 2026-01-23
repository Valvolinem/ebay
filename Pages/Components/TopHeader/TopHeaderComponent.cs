using OpenQA.Selenium;

namespace Ebay.Automation.Framework.Pages.Components.TopHeader
{
    public class TopHeaderComponent
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _root;

        public SearchSection Search { get; }
        public NavigationSection Navigation { get; }

        public TopHeaderComponent(IWebDriver driver)
        {
            _driver = driver;
            _root = driver.FindElement(By.Id("globalHeaderWrapper"));
            
            Search = new SearchSection(_root, _driver);
            Navigation = new NavigationSection(_root, _driver);
        }
    }
}
