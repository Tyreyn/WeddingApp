using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace TestsLibrary.Frontend
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class MainPage
    {
        [Test]
        public async Task LoginWithCorrectCredientals()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync("http://3.71.218.200/");

            await page.Locator("input[type=\"text\"]").First.FillAsync("Michał");

            await page.Locator("input[type=\"text\"]").Nth(1).FillAsync("123");

            await page.GetByRole(AriaRole.Button, new() { NameString = "Zaloguj" }).ClickAsync();

            await page.Locator("label:has-text(\"Dodaj zdjęcie\")").IsVisibleAsync();
        }
    }
}
