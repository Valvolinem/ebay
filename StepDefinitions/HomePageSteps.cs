using Ebay.Automation.Framework.Pages.Constants;
using Ebay.Automation.Framework.Utilities;
using Serilog;
using TechTalk.SpecFlow;

namespace Ebay.Automation.Framework.StepDefinitions
{
    [Binding]
    public class HomePageSteps
    {
        private readonly PageObjectManager _pages;
        private readonly ScenarioContext _context;

        public HomePageSteps(PageObjectManager pages, ScenarioContext context)
        {
            _pages = pages;
            _context = context;
        }

        [Given(@"I navigate to eBay home page")]
        public void GivenINavigateToEBayHomePage()
        {
            _pages.HomePage.NavigateTo(Urls.EbayHomePageUrl);
            Log.Information("Navigated to eBay home page");
        }

        [Given(@"I verify home page is loaded")]
        public void GivenIVerifyHomePageIsLoaded()
        {
            var title = _pages.HomePage.GetLogoTitle();
            Log.Information("Page title: {Title}", title);
            Assert.That(title, Is.EqualTo("eBay Home"));
        }

        [Given(@"I set shipping country to ""(.*)""")]
        public void GivenISetShippingCountryTo(string country)
        {
            _pages.HomePage.Header.Navigation.ChangeShippingCountry(country);
            _context["ShippingCountry"] = country;
            Log.Information("Shipping country set to: {Country}", country);
        }

        [When(@"I select ""(.*)"" category")]
        public void WhenISelectCategory(string category)
        {
            _pages.HomePage.Header.Search.SelectCategory(EbayCategory.ToysAndHobbies);
            Log.Information("Selected category: {Category}", category);
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchTerm)
        {
            _pages.HomePage.Header.Search.EnterSearchText(searchTerm);
            _pages.HomePage.Header.Search.ClickSearchButton();
            _context["SearchTerm"] = searchTerm;
            Log.Information("Searched for: {SearchTerm}", searchTerm);
        }
    }
}
