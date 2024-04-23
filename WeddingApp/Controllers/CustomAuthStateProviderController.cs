namespace WeddingApp.Controllers
{
    using System.Security.Claims;
    using Blazored.LocalStorage;
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
        NavigationManager navigationManager,
        ILocalStorageService localStorageService,
        CustomAuthState customAuthState) : AuthenticationStateProvider
    {

        /// <summary>
        /// Gets or sets access to current http context accessor.
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; } = httpContextAccessor;

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

        public ClaimsPrincipal currentUser;
        public Guid id = Guid.NewGuid();

        /// <inheritdoc/>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            currentUser = await this.CustomAuthState.GetCurrentUser();
            Console.WriteLine($"{id} {(currentUser.Identity == null ? string.Empty : currentUser.Identity.Name)}");
            Tuple<bool, string> loginSuccess =
                this.SqlServerDataController.CheckIfUserExistsInDatabaseAndDataCorrectness(this.currentUser).Result;

            if (loginSuccess.Item1 &&
                loginSuccess.Item2 == string.Empty)
            {
                this.NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(this.currentUser)));
                return new AuthenticationState(this.currentUser);
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
                await LocalStorageService.SetItemAsStringAsync("AuthTokenName", userName);
                await LocalStorageService.SetItemAsStringAsync("AuthTokenPhoneNumber", userPhoneNumber);
                this.NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(user)));
            }

            if (!loginSuccess.Item1 && loginSuccess.Item2 != string.Empty)
            {
                Console.WriteLine("User does not exists in database");

                await this.SqlServerDataController.AddUserToDatabase(userPhoneNumber, userName);
            }

            return loginSuccess;
        }

        /// <summary>
        /// Get current browser user.
        /// </summary>
        /// <returns>
        /// Current browser user.
        /// </returns>
        public async Task<ClaimsPrincipal?> GetCurrentUserAsync()
        {
            string userName = string.Empty, userPhoneNumber = string.Empty;
            try
            {
                userName = await LocalStorageService.GetItemAsync<string>("AuthTokenName");
                userPhoneNumber = await LocalStorageService.GetItemAsync<string>("AuthTokenPhoneNumber");
            }
            catch
            {
                //await LocalStorageService.ClearAsync();
                return new ClaimsPrincipal();
            }

            if (userName == null || userPhoneNumber == null) { return new ClaimsPrincipal(); }
            ClaimsIdentity identity = new ClaimsIdentity(
                new[]
            {
                                    new Claim(ClaimTypes.Name, userName),
                                    new Claim(ClaimTypes.MobilePhone, userPhoneNumber),
            },
                CookieAuthenticationDefaults.AuthenticationScheme);

            this.currentUser = new ClaimsPrincipal(identity);
            this.CustomAuthState.SetCurrentUser(this.currentUser);
            return new ClaimsPrincipal(identity);
        }
    }
}
