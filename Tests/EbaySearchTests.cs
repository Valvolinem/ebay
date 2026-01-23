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
        public void SearchForMonopolyInToysAndHobbiesCategory()
        {
            var productName = "Monopoly: Elf Edition Board Game";
            var homePageTitle = "eBay Home";
            var productPrice = "$39.99";

            Pages.HomePage.NavigateTo(Urls.EbayHomePage);
            string logoTitle = Pages.HomePage.GetLogoTitle();
            Assert.That(logoTitle, Is.EqualTo(homePageTitle), "Page title is not correct!");
            Pages.HomePage.Header.Navigation.ChangeShippingCountry(CountryConstants.Bulgaria);
            Pages.HomePage.Header.Search.SelectCategory(EbayCategory.ToysAndHobbies);
            Pages.HomePage.Header.Search.EnterSearchText(productName);
            Pages.HomePage.Header.Search.ClickSearchButton();
            Pages.ProductResultPage.ChangeListingView(ProductListView.List);
            var title = Pages.ProductResultPage.GetProductTitle(0);
            Console.WriteLine("The title of the first product is: " + title.Text);
            Assert.AreEqual(productName, title.Text);
            var price = Pages.ProductResultPage.GetProductPrice(0);
            Console.WriteLine("The price of the first product is: " + price.Text);
            Assert.AreEqual(productPrice, price.Text);
            Pages.ProductResultPage.ClickProductByIndex(0);
            Pages.DetailedProductPage.SwitchToLastWindow();
            var detailedProductTitle = Pages.DetailedProductPage.GetProductTitle();
            Assert.AreEqual(productName, detailedProductTitle);
            Assert.That(detailedProductTitle, Does.Contain(productName));
            var detailedProductPrice = Pages.DetailedProductPage.GetProductPrice();
            Assert.AreEqual(productPrice, detailedProductPrice);
            Pages.DetailedProductPage.SetQuantity(2);
            Pages.DetailedProductPage.ClickSeeDetailsBtn();
            bool hasBulgaria = Pages.DetailedProductPage.IsCountryInShipsList("Bulgaria");
            Assert.That(hasBulgaria, Is.True);
            Pages.DetailedProductPage.ClickModalTab("Payment methods");
            bool hasVisa = Pages.DetailedProductPage.IsPaymentMethodAvailable("Visa");
            Assert.That(hasVisa, Is.True);
            Pages.DetailedProductPage.CloseModal();
            Pages.DetailedProductPage.ClickActionButton("Add to cart");
            Pages.DetailedProductPage.ClickSeeInCart();
            Pages.DetailedProductPage.WaitForPageLoad();
            bool isInCart = Pages.DetailedProductPage.IsUrlContaining("cart.ebay.com");
            Assert.That(isInCart, Is.True);
            var quantityInCart = Pages.CartPage.GetProductQuantity();
            Assert.That(quantityInCart, Is.EqualTo("2"));
            var totalSumInCart = Pages.CartPage.GetProductTotalSum();
            Assert.That(totalSumInCart, Is.EqualTo("US $79.98"));
        }
    }
}
