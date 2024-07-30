﻿namespace WeddingApp.Controllers
{
    using System.Security.Claims;
    using WeddingAppDTO.DataTransferObject;

    public class CustomAuthState
    {
        /// <summary>
        /// Gets or sets current user claims.
        /// </summary>
        public ClaimsPrincipal CurrentUserClaims { get; set; } = new ClaimsPrincipal();

        /// <summary>
        /// Gets or sets current user entity.
        /// </summary>
        public UserDto CurrentUserEntity { get; set; } = new UserDto { UserName = null, UserPhone = null};

    }
}
