namespace WeddingApp.Entities
{
    /// <summary>
    /// The picture entity.
    /// </summary>
    public class PictureEntity
    {
        /// <summary>
        /// The user id.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The picture local path.
        /// </summary>
        public string PicturePath { get; set; }

        /// <summary>
        /// The picture add time.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
