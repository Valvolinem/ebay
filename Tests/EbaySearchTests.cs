using Ebay.Automation.Framework.Pages.Constants;
using Ebay.Automation.Framework.Utilities;

namespace Ebay.Automation.Framework.Tests
{
    /// <summary>
    /// eBay search functionality tests.
    /// </summary>
    public class EbaySearchTests : BaseSeleniumTest
    {
        [Test]
        [Retry(3)]
        public void SearchForMonopolyInToysAndHobbiesCategory()
        {
            var productName = "Monopoly: Elf Edition Board Game";
            var homePageTitle = "eBay Home";
            var productPrice = "$39.99";
            var productIndex = 1;
            var productQuantity = 2;
            var twoProductsTotalPrice = "US $79.98";

            Pages.HomePage.NavigateTo(Urls.EbayHomePageUrl);

            string logoTitle = Pages.HomePage.GetLogoTitle();
            Assert.That(logoTitle, Is.EqualTo(homePageTitle), "Page title is not correct!");
            Pages.HomePage.Header.Navigation.ChangeShippingCountry(CountryConstants.Bulgaria);
            Pages.HomePage.Header.Search.SelectCategory(EbayCategory.ToysAndHobbies);
            Pages.HomePage.Header.Search.EnterSearchText(productName);
            Pages.HomePage.Header.Search.ClickSearchButton();

            Pages.ProductResultPage.ChangeListingView(ProductListView.List);
            var title = Pages.ProductResultPage.GetProductTitle(productIndex);
            Console.WriteLine("The title of the first product is: " + title.Text);
            Assert.AreEqual(productName, title.Text);
            var price = Pages.ProductResultPage.GetProductPrice(productIndex);
            Console.WriteLine("The price of the first product is: " + price.Text);
            Assert.AreEqual(productPrice, price.Text);
            Pages.ProductResultPage.ClickProductByIndex(productIndex);
            Pages.DetailedProductPage.SwitchToLastWindow();

            var detailedProductTitle = Pages.DetailedProductPage.GetProductTitle();
            Assert.AreEqual(productName, detailedProductTitle);
            Assert.That(detailedProductTitle, Does.Contain(productName));
            var detailedProductPrice = Pages.DetailedProductPage.GetProductPrice();
            Assert.AreEqual(productPrice, detailedProductPrice);
            Pages.DetailedProductPage.SetQuantity(productQuantity);

            Pages.DetailedProductPage.ClickSeeDetailsBtn();
            bool hasBulgaria = Pages.DetailedProductPage.IsCountryInShipsList(CountryConstants.Bulgaria);
            Assert.That(hasBulgaria, Is.True);
            Pages.DetailedProductPage.ClickModalTab(ShipModalTabs.PaymentMethods);
            bool hasVisa = Pages.DetailedProductPage.IsPaymentMethodAvailable(PaymentTypes.Visa);
            Assert.That(hasVisa, Is.True);
            Pages.DetailedProductPage.CloseModal();

            Pages.DetailedProductPage.ClickActionButton(OrderButtons.AddToCart);
            Pages.DetailedProductPage.ClickSeeInCart();
            Pages.DetailedProductPage.WaitForPageLoad();

            bool isInCart = Pages.DetailedProductPage.IsUrlContaining(Urls.EbayCartPageUrl);
            Assert.That(isInCart, Is.True);
            var quantityInCart = Pages.CartPage.GetProductQuantity();
            Assert.That(quantityInCart, Is.EqualTo(productQuantity.ToString()));
            var totalSumInCart = Pages.CartPage.GetProductTotalSum();
            Assert.That(totalSumInCart, Is.EqualTo(twoProductsTotalPrice));
        }
    }
}
