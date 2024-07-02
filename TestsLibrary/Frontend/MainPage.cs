using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using WeddingApp.Data.Entities;

namespace TestsLibrary.Frontend
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class MainPage
    {
        public IConfiguration configuration;

        public string pageUrl = string.Empty;
        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<ConnectionStringClass>()
                .AddEnvironmentVariables()
                .Build();
            pageUrl = this.configuration.GetSection("AppUrl:Url").Value;
        }

        [Test]
        public async Task LoginWithCorrectCredientials()
        {
            Console.WriteLine("Start new browser");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            Console.WriteLine("Start page");
            await page.GotoAsync(pageUrl);

            Console.WriteLine("Start put correct name");
            await page.Locator("input[type=\"text\"]").First.FillAsync("Michał");

            Console.WriteLine("Start put correct number");
            await page.Locator("input[type=\"text\"]").Nth(1).FillAsync("123");

            Console.WriteLine("Click Zaloguj button");
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zaloguj" }).ClickAsync();

            Console.WriteLine("Check if there is Dodaj zdjęcie button");
            await page.Locator("label:has-text(\"Dodaj zdjęcie\")").IsVisibleAsync();
        }

        [Test]
        public async Task LoginWithWrongCredientials()
        {
            Console.WriteLine("Start new browser");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            Console.WriteLine("Start page");
            await page.GotoAsync(pageUrl);

            Console.WriteLine("Start put correct name");
            await page.Locator("input[type=\"text\"]").First.FillAsync("WrongName");

            Console.WriteLine("Start put correct number");
            await page.Locator("input[type=\"text\"]").Nth(1).FillAsync("123");

            Console.WriteLine("Click Zaloguj button");
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zaloguj" }).ClickAsync();

            Console.WriteLine("Check if there is Error message");
            await page.GetByText("Imię nie zgadza się z podanym numerem telefonu!").IsVisibleAsync();

        }

        [Test]
        public async Task LoginWithWrongCredientialsThenWithCorrect()
        {
            Console.WriteLine("Start new browser");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            Console.WriteLine("Start page");
            await page.GotoAsync(pageUrl);

            Console.WriteLine("Start put correct name");
            await page.Locator("input[type=\"text\"]").First.FillAsync("WrongName");

            Console.WriteLine("Start put correct number");
            await page.Locator("input[type=\"text\"]").Nth(1).FillAsync("123");

            Console.WriteLine("Click Zaloguj button");
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zaloguj" }).ClickAsync();

            Console.WriteLine("Check if there is Error message");
            await page.GetByText("Imię nie zgadza się z podanym numerem telefonu!").IsVisibleAsync();

            Console.WriteLine("Start put correct name");
            await page.Locator("input[type=\"text\"]").First.FillAsync("Michał");

            Console.WriteLine("Click Zaloguj button");
            await page.GetByRole(AriaRole.Button, new() { NameString = "Zaloguj" }).ClickAsync();

            Console.WriteLine("Check if there is Dodaj zdjęcie button");
            await page.Locator("label:has-text(\"Dodaj zdjęcie\")").IsVisibleAsync();
        }
    }
}
