using Ebay.Automation.Framework.Pages.Constants;
using Ebay.Automation.Framework.Utilities;
using OpenQA.Selenium;
using Serilog;
using TechTalk.SpecFlow;

namespace Ebay.Automation.Framework.StepDefinitions
{
    [Binding]
    public class ProductSearchSteps
    {
        private readonly PageObjectManager _pages;
        private readonly ScenarioContext _context;

        public ProductSearchSteps(PageObjectManager pages, ScenarioContext context)
        {
            _pages = pages;
            _context = context;
        }

        [When(@"I switch to List view")]
        public void WhenISwitchToListView()
        {
            _pages.ProductResultPage.ChangeListingView(ProductListView.List);
            Log.Information("Switched to List view");
        }

        [Then(@"I should see product (.*) with title ""(.*)""")]
        public void ThenIShouldSeeProductWithTitle(int productIndex, string expectedTitle)
        {
            var titleElement = _pages.ProductResultPage.GetProductTitle(productIndex);
            var titleText = titleElement.Text;
            _context[$"Product{productIndex}Title"] = titleText;
            _context[$"Product{productIndex}Index"] = productIndex;
            
            Log.Information("Product {Index} title: {Title}", productIndex, titleText);
            Assert.That(titleText, Is.EqualTo(expectedTitle));
        }

        [Then(@"I should see product (.*) with price ""(.*)""")]
        public void ThenIShouldSeeProductWithPrice(int productIndex, string expectedPrice)
        {
            var priceElement = _pages.ProductResultPage.GetProductPrice(productIndex);
            var priceText = priceElement.Text;
            _context[$"Product{productIndex}Price"] = priceText;
            
            Log.Information("Product {Index} price: {Price}", productIndex, priceText);
            Assert.That(priceText, Is.EqualTo(expectedPrice));
        }

        [When(@"I open product (.*) details")]
        public void WhenIOpenProductDetails(int productIndex)
        {
            // Use fresh locator to avoid stale element
            _pages.ProductResultPage.ClickProductByIndex(productIndex - 1);
            _pages.DetailedProductPage.SwitchToLastWindow();
            Log.Information("Opened product {Index} details", productIndex);
        }
    }
}
