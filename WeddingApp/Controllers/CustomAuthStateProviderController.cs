namespace WeddingApp.Controllers
{
    using System.Security.Claims;
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Http;
    using WeddingApp.Entities;

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
        SqlServerDataController sqlServerDataController,
        NavigationManager navigationManager,
        ILocalStorageService localStorageService,
        CustomAuthState customAuthState) : AuthenticationStateProvider
    {

        /// <summary>
        /// Gets or sets access to SQL server data access.
        /// </summary>
        public SqlServerDataController SqlServerDataController { get; set; } = sqlServerDataController;

        public ILocalStorageService LocalStorageService { get; set; } = localStorageService;

        /// <summary>
        /// Gets or sets access to navigation manager.
        /// </summary>
        public NavigationManager NavigationManager { get; set; } = navigationManager;

        public CustomAuthState CustomAuthState { get; set; } = customAuthState;

        public Guid id = Guid.NewGuid();

        /// <inheritdoc/>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            Console.WriteLine($"{id} {(this.CustomAuthState.CurrentUserClaims.Identity == null
                ? string.Empty
                : this.CustomAuthState.CurrentUserClaims.Identity.Name)}");

            Tuple<bool, string> loginSuccess =
                this.SqlServerDataController.CheckIfUserExistsInDatabaseAndDataCorrectness(this.CustomAuthState.CurrentUserClaims).Result;

            if (loginSuccess.Item1 &&
                loginSuccess.Item2 == string.Empty)
            {
                this.NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(this.CustomAuthState.CurrentUserClaims)));
                return new AuthenticationState(this.CustomAuthState.CurrentUserClaims);
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
            //await LocalStorageService.ClearAsync();
            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            Tuple<bool, string> loginSuccess = await
                this.SqlServerDataController.CheckIfUserExistsInDatabaseAndDataCorrectness(user);


            if ((loginSuccess.Item1 && loginSuccess.Item2 == string.Empty) || (!loginSuccess.Item1 && loginSuccess.Item2 != string.Empty))
            {
                await this.LocalStorageService.SetItemAsStringAsync("AuthTokenName", userName);
                await this.LocalStorageService.SetItemAsStringAsync("AuthTokenPhoneNumber", userPhoneNumber);
                this.NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(user)));
                await this.SetCurrentUser(userPhoneNumber, user);
            }

            if (!loginSuccess.Item1 && loginSuccess.Item2 != string.Empty)
            {
                Console.WriteLine("User does not exists in database");

                await this.SqlServerDataController.AddUserToDatabase(userPhoneNumber, userName);
                await this.SetCurrentUser(userPhoneNumber, user);
            }

            return loginSuccess;
        }

        /// <summary>
        /// Get current browser user.
        /// </summary>
        /// <returns>
        /// Current browser user.
        /// </returns>
        public async Task GetCurrentUserAsync()
        {
            string userName = string.Empty, userPhoneNumber = string.Empty;
            try
            {
                userName = await this.LocalStorageService.GetItemAsync<string>("AuthTokenName");
                userPhoneNumber = await this.LocalStorageService.GetItemAsync<string>("AuthTokenPhoneNumber");
            }
            catch
            {
                await this.SetCurrentUser(userPhoneNumber, new ClaimsPrincipal());
                return;
            }

            if (userName == null || userPhoneNumber == null)
            {
                await this.SetCurrentUser(userPhoneNumber, new ClaimsPrincipal());
                return;
            }

            ClaimsIdentity identity = new ClaimsIdentity(
                new[]
            {
                                    new Claim(ClaimTypes.Name, userName),
                                    new Claim(ClaimTypes.MobilePhone, userPhoneNumber),
            },
                CookieAuthenticationDefaults.AuthenticationScheme);

            await this.SetCurrentUser(userPhoneNumber, new ClaimsPrincipal(identity));
        }

        private async Task SetCurrentUser(string userPhoneNumber, ClaimsPrincipal? claims = null)
        {
            this.CustomAuthState.CurrentUserClaims = claims;
            this.CustomAuthState.CurrentUserEntity = await this.SqlServerDataController.GetUserEntity(userPhoneNumber);
        }
    }
}
