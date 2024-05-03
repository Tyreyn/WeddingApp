namespace WeddingApp.Entities
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The user entity.
    /// </summary>
    public class UserEntity
    {

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać imię")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets user phone number.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać numer telefonu")]
        public string UserPhone { get; set; }
    }
}
