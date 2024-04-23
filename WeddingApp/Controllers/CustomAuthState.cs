using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace WeddingApp.Controllers
{
    public class CustomAuthState
    {
        public ILocalStorageService LocalStorageService;

        public ClaimsPrincipal currentUser { get; set; } = new ClaimsPrincipal();

        public async Task SetCurrentUser(ClaimsPrincipal currentUser)
        {
            this.currentUser = currentUser;
        }

        public async Task<ClaimsPrincipal> GetCurrentUser()
        {
            return this.currentUser;
        }

    }
}
