using Dapper;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data.Context;
using WeddingApp.Data.Entities;
using WeddingApp.Helpers.SqlCommands;

namespace WeddingApp.Data.Operations
{
    public class PictureOperations
    {
        private WeddingAppUserContext Context { get; set; }

        public PictureOperations(WeddingAppUserContext weddingAppUserContext)
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
            PictureEntity pictureEntity = new PictureEntity { PicturePath = pathToPicture, UserID = userID };
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
        public Task<List<PictureEntity>> GetAllPictures()
        {
            List<PictureEntity> pictureEntities = this.Context.Pictures.AsNoTracking().AsList();
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
            PictureEntity pictureEntity = new PictureEntity { PicturePath = pathToPicture };
            this.Context.Pictures.Remove(pictureEntity);
            Console.WriteLine($"{pathToPicture} deleted from database");
            this.Context.SaveChanges();
            return Task.FromResult(true);

        }
    }
}
