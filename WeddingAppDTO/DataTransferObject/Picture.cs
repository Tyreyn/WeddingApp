using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingAppDTO.DataTransferObject
{
    /// <summary>
    /// The picture entity.
    /// </summary>
    public class Picture
    {
        /// <summary>
        /// The user id.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The picture local path.
        /// </summary>
        [Key]
        public string PicturePath { get; set; }

        /// <summary>
        /// The picture add time.
        /// </summary>
        public DateTime? TimeStamp { get; set; }
    }
}
