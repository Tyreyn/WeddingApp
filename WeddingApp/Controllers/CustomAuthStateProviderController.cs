namespace WeddingApp.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Cookie authentication state provider.
    /// </summary>
    /// <param name="httpContextAccessor">
    /// Provides access to current http context accessor.
    /// </param>
    /// <param name="SqlServerDataController">
    /// Provides access to SQL server data controller.
    /// </param>
    public class CustomAuthStateProviderController(
        IHttpContextAccessor httpContextAccessor,
        SqlServerDataController sqlServerDataController,
        NavigationManager navigationManager) : AuthenticationStateProvider
    {

        /// <summary>
        /// Gets or sets access to current http context accessor.
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; } = httpContextAccessor;

        /// <summary>
        /// Gets or sets access to SQL server data access.
        /// </summary>
        public SqlServerDataController SqlServerDataController { get; set; } = sqlServerDataController;

        /// <summary>
        /// Gets or sets access to navigation manager.
        /// </summary>
        public NavigationManager NavigationManager { get; set; } = navigationManager;

        /// <inheritdoc/>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsPrincipal? user = await this.GetCurrentUserAsync();
            Tuple<bool, string> loginSuccess =
                this.SqlServerDataController.CheckIfUserExistsInDatabaseAndDataCorrectness(user).Result;

            if (loginSuccess.Item1 &&
                loginSuccess.Item2 == string.Empty)
            {
                return new AuthenticationState(user);
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        /// <summary>
        /// Authenticate user and add him to database if it is not already.
        /// </summary>
        /// <param name="userName">
        /// User name.
        /// </param>
        /// <param name="userPhoneNumber">
        /// User phone number.
        /// </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<Tuple<bool, string>> AuthenticateUser(string userName, string userPhoneNumber)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.MobilePhone, userPhoneNumber),
            },
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            Tuple<bool, string> loginSuccess = await
                this.SqlServerDataController.CheckIfUserExistsInDatabaseAndDataCorrectness(user);

            if (loginSuccess.Item1 && loginSuccess.Item2 == string.Empty)
            {
                Console.WriteLine("User already exists in database");
                await this.HttpContextAccessor.HttpContext.SignInAsync(user);
                this.NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(user)));
            }
            else if (!loginSuccess.Item1 && loginSuccess.Item2 != string.Empty)
            {
                Console.WriteLine("User does not exists in database");
                await this.SqlServerDataController.AddUserToDatabase(userPhoneNumber, userName);
                await this.HttpContextAccessor.HttpContext.SignInAsync(user);
                this.NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(user)));
            }

            return loginSuccess;
        }

        /// <summary>
        /// Get current browser user.
        /// </summary>
        /// <returns>
        /// Current browser user.
        /// </returns>
        public Task<ClaimsPrincipal?> GetCurrentUserAsync() => Task.FromResult(this.HttpContextAccessor.HttpContext.User);
    }
}
