using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Ebay.Automation.Framework.Utilities;

namespace Ebay.Automation.Framework.Tests
{
    /// <summary>
    /// Base class for all Selenium tests.
    /// Handles WebDriver initialization, cleanup, and provides shared utilities.
    /// </summary>
    public abstract class BaseSeleniumTest
    {
        protected IWebDriver Driver { get; private set; } = null!;
        protected PageObjectManager Pages { get; private set; } = null!;

        [SetUp]
        public void BaseSetUp()
        {
            // Initialize WebDriver
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();

            // Initialize PageObjectManager
            Pages = new PageObjectManager(Driver);
        }

        [TearDown]
        public void BaseTearDown()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
