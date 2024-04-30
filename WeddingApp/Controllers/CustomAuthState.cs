namespace WeddingApp.Controllers
{
    using System.Security.Claims;
    using WeddingApp.Entities;

    public class CustomAuthState
    {
        /// <summary>
        /// Gets or sets current user claims.
        /// </summary>
        public ClaimsPrincipal CurrentUserClaims { get; set; } = new ClaimsPrincipal();

        /// <summary>
        /// Gets or sets current user entity.
        /// </summary>
        public UserEntity CurrentUserEntity { get; set; } = new UserEntity();

    }
}
