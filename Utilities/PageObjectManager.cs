using OpenQA.Selenium;
using Ebay.Automation.Framework.Pages;

namespace Ebay.Automation.Framework.Utilities
{
    /// <summary>
    /// PageObjectManager with lazy initialization.
    /// </summary>
    public class PageObjectManager
    {
        private readonly IWebDriver _driver;

        private HomePage? _homePage;
        private ProductResultPage? _productResultPage;

        private DetailedProductPage? _detailedProductPage;

        private CartPage? _cartPage;


        public PageObjectManager(IWebDriver driver)
        {
            _driver = driver;
        }

        public HomePage HomePage => _homePage ??= new HomePage(_driver);
        public ProductResultPage ProductResultPage => _productResultPage ??= new ProductResultPage(_driver);
        public DetailedProductPage DetailedProductPage => _detailedProductPage ??= new DetailedProductPage(_driver);
        public CartPage CartPage => _cartPage ??= new CartPage(_driver);
    }
}
