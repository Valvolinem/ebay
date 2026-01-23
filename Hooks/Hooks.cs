using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Ebay.Automation.Framework.Utilities;
using TechTalk.SpecFlow;
using BoDi;
using Serilog;

namespace Ebay.Automation.Framework.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _container;
        private IWebDriver? _driver;

        public Hooks(IObjectContainer container)
        {
            _container = container;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/test-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            
            _container.RegisterInstanceAs(_driver);
            _container.RegisterInstanceAs(new PageObjectManager(_driver));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Log.CloseAndFlush();
        }
    }
}
