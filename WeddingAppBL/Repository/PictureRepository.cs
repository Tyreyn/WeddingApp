using Microsoft.EntityFrameworkCore;
using System.Linq;
using WeddingAppDTO.Context;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppBL.Repository
{
    public class PictureRepository
    {
        private WeddingAppUserContext Context { get; set; }

        public PictureRepository(WeddingAppUserContext weddingAppUserContext)
        {
            this.Context = weddingAppUserContext;
        }

        /// <summary>
        /// Add new picture entity to database.
        /// </summary>
        /// <param name="userID">
        /// ID of user who is adding new picture.
        /// </param>
        /// <param name="pathToPicture">
        /// Path to new picture.
        /// </param>
        /// <returns>
        /// True, if added correctly.
        /// </returns>
        public Task<bool> AddPictureToDatabase(int userID, string pathToPicture)
        {
            Console.WriteLine("Starting adding picture to database");
            PictureDto pictureEntity = new PictureDto { PicturePath = pathToPicture, UserID = userID, TimeStamp = DateTime.UtcNow };
            this.Context.Pictures.Add(pictureEntity);
            this.Context.SaveChanges();
            Console.WriteLine($"User: {userID} added successfully picture {pathToPicture} to database");
            return Task.FromResult(true);
        }

        /// <summary>
        /// Get all pictures entities from database.
        /// </summary>
        /// <returns>
        /// List of picture entities.
        /// </returns>
        public Task<List<PictureDto>> GetAllPictures()
        {
            List<PictureDto> pictureEntities = this.Context.Pictures.AsNoTracking().ToList();
            return Task.FromResult(pictureEntities);
        }

        /// <summary>
        /// Delete picture from database.
        /// </summary>
        /// <param name="pathToPicture">
        /// Path to picture.
        /// </param>
        /// <returns>
        /// True, if deleted correctly.
        /// </returns>
        public Task<bool> DeletePicture(string pathToPicture)
        {
            Context.ChangeTracker.Clear();
            Console.WriteLine("Starting deleting picture to database");
            PictureDto pictureEntity = new PictureDto { PicturePath = pathToPicture };
            this.Context.Pictures.Remove(pictureEntity);
            Console.WriteLine($"{pathToPicture} deleted from database");
            this.Context.SaveChanges();
            return Task.FromResult(true);

        }
    }
}
