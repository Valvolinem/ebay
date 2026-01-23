using Ebay.Automation.Framework.Utilities;
using Serilog;
using TechTalk.SpecFlow;

namespace Ebay.Automation.Framework.StepDefinitions
{
    [Binding]
    public class CartSteps
    {
        private readonly PageObjectManager _pages;
        private readonly ScenarioContext _context;

        public CartSteps(PageObjectManager pages, ScenarioContext context)
        {
            _pages = pages;
            _context = context;
        }

        [Then(@"cart should contain (.*) items")]
        public void ThenCartShouldContainItems(int expectedQuantity)
        {
            var isInCart = _pages.DetailedProductPage.IsUrlContaining(Urls.EbayCartPageUrl);
            Assert.That(isInCart, Is.True, "Should be on cart page");

            var actualQuantity = _pages.CartPage.GetProductQuantity();
            Log.Information("Cart quantity: {Quantity}", actualQuantity);
            
            Assert.That(actualQuantity, Is.EqualTo(expectedQuantity.ToString()));
        }

        [Then(@"cart total should be ""(.*)""")]
        public void ThenCartTotalShouldBe(string expectedTotal)
        {
            var actualTotal = _pages.CartPage.GetProductTotalSum();
            Log.Information("Cart total: {Total}", actualTotal);
            
            Assert.That(actualTotal, Is.EqualTo(expectedTotal));
        }
    }
}
