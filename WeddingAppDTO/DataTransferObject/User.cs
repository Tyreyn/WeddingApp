namespace WeddingAppDTO.DataTransferObject
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The user entity.
    /// </summary>
    [Index(nameof(UserPhone), IsUnique = true)]
    public class User
    {

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        [Key]
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać imię")]
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets user phone number.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać numer telefonu")]
        public required string UserPhone { get; set; }

        public ICollection<Picture> Posts { get; } = new List<Picture>();
    }
}
