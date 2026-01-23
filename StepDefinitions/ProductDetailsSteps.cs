using Ebay.Automation.Framework.Pages.Constants;
using Ebay.Automation.Framework.Utilities;
using Serilog;
using TechTalk.SpecFlow;

namespace Ebay.Automation.Framework.StepDefinitions
{
    [Binding]
    public class ProductDetailsSteps
    {
        private readonly PageObjectManager _pages;
        private readonly ScenarioContext _context;

        public ProductDetailsSteps(PageObjectManager pages, ScenarioContext context)
        {
            _pages = pages;
            _context = context;
        }

        [Then(@"product details should show title ""(.*)""")]
        public void ThenProductDetailsShouldShowTitle(string expectedTitle)
        {
            var actualTitle = _pages.DetailedProductPage.GetProductTitle();
            Log.Information("Product details title: {Title}", actualTitle);
            
            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
            Assert.That(actualTitle, Does.Contain(expectedTitle));
        }

        [Then(@"product details should show price ""(.*)""")]
        public void ThenProductDetailsShouldShowPrice(string expectedPrice)
        {
            var actualPrice = _pages.DetailedProductPage.GetProductPrice();
            Log.Information("Product details price: {Price}", actualPrice);
            
            Assert.That(actualPrice, Is.EqualTo(expectedPrice));
        }

        [When(@"I set quantity to (.*)")]
        public void WhenISetQuantityTo(int quantity)
        {
            _pages.DetailedProductPage.SetQuantity(quantity);
            _context["Quantity"] = quantity;
            Log.Information("Quantity set to: {Quantity}", quantity);
        }

        [When(@"I view shipping details")]
        public void WhenIViewShippingDetails()
        {
            _pages.DetailedProductPage.ClickSeeDetailsBtn();
            Log.Information("Viewing shipping details");
        }

        [Then(@"shipping should be available to ""(.*)""")]
        public void ThenShippingShouldBeAvailableTo(string country)
        {
            var isAvailable = _pages.DetailedProductPage.IsCountryInShipsList(country);
            Log.Information("Shipping to {Country}: {Available}", country, isAvailable);
            
            Assert.That(isAvailable, Is.True, $"Shipping to {country} should be available");
        }

        [When(@"I view payment methods")]
        public void WhenIViewPaymentMethods()
        {
            _pages.DetailedProductPage.ClickModalTab(ShipModalTabs.PaymentMethods);
            Log.Information("Viewing payment methods");
        }

        [Then(@"""(.*)"" should be accepted")]
        public void ThenPaymentMethodShouldBeAccepted(string paymentMethod)
        {
            var isAccepted = _pages.DetailedProductPage.IsPaymentMethodAvailable(PaymentTypes.Visa);
            Log.Information("Payment method {Method}: {Accepted}", paymentMethod, isAccepted);
            
            Assert.That(isAccepted, Is.True, $"{paymentMethod} should be accepted");
        }

        [Then(@"I close shipping modal")]
        public void ThenICloseShippingModal()
        {
            _pages.DetailedProductPage.CloseModal();
            Log.Information("Shipping modal closed");
        }

        [When(@"I add to cart")]
        public void WhenIAddToCart()
        {
            _pages.DetailedProductPage.ClickActionButton(OrderButtons.AddToCart);
            Log.Information("Added product to cart");
        }

        [When(@"I open cart")]
        public void WhenIOpenCart()
        {
            _pages.DetailedProductPage.ClickSeeInCart();
            _pages.DetailedProductPage.WaitForPageLoad();
            Log.Information("Opened cart");
        }
    }
}
