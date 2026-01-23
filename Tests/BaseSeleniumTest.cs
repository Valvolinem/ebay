using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Ebay.Automation.Framework.Utilities;
using Serilog;

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
        protected ILogger Logger { get; private set; } = null!;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/test-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        [SetUp]
        public void BaseSetUp()
        {
            Logger = Log.ForContext(GetType());
            Logger.Information("Starting test: {TestName}", TestContext.CurrentContext.Test.Name);

            // Initialize WebDriver
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();

            // Initialize PageObjectManager
            Pages = new PageObjectManager(Driver);
        }

        [TearDown]
        public void BaseTearDown()
        {
            Logger.Information("Ending test: {TestName}", TestContext.CurrentContext.Test.Name);
            Driver.Quit();
            Driver.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Log.CloseAndFlush();
        }
    }
}
