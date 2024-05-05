namespace WeddingApp.Controllers
{
    using System.Security.Claims;
    using Dapper;
    using WeddingApp.Context;
    using WeddingApp.Entities;
    using WeddingApp.Helpers.SqlCommands;

    /// <summary>
    /// Sql server data controller.
    /// </summary>
    /// <param name="sqlServerDataAccess">
    /// Provides SQL server data access.
    /// </param>
    public class SqlServerDataController(SqlServerDataAccess sqlServerDataAccess)
    {
        /// <summary>
        /// Gets or sets SQL server data access.
        /// </summary>
        public SqlServerDataAccess SqlServerDataAccess { get; set; } = sqlServerDataAccess;

        /// <summary>
        /// Checking if specified user is already in database.
        /// </summary>
        /// <param name="userFromCookies">
        /// User claims principal from cookies.
        /// </param>
        /// <returns>
        /// Tuple:
        /// Item 1(bool): True, if user exists in database
        /// Item 2(string): Empty string when no user in cookies otherwise information about data correctness.
        /// </returns>
        public Task<Tuple<bool, string>> CheckIfUserExistsInDatabaseAndDataCorrectness(ClaimsPrincipal userFromCookies)
        {
            if (!userFromCookies.Claims.Any())
            {
                return Task.FromResult(new Tuple<bool, string>(false, string.Empty));
            }

            string userPhone = userFromCookies.Claims.First(
                claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone").Value;
            string userName = userFromCookies.Claims.First(
                claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            UserEntity userFromDatabase = this.GetUserEntity(userPhone).Result;

            if (userFromDatabase.UserName == null)
            {
                return Task.FromResult(new Tuple<bool, string>(
                    false,
                    "User does not exists in database"));
            }

            string message = this.CheckIfDataIsCorrect(userFromDatabase, userPhone, userName).Result;
            return Task.FromResult(new Tuple<bool, string>(
                true,
                message));
        }

        /// <summary>
        /// Get user entity from database.
        /// </summary>
        /// <param name="userPhoneNumber">
        /// User phone number to get.
        /// </param>
        /// <returns>
        /// User Entity.
        /// </returns>
        public async Task<UserEntity> GetUserEntity(string userPhoneNumber)
        {
            List<UserEntity> userFromDatabase =
                this.SqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(
                    SqlCommands.SelectUserByPhoneNumber,
                    new DynamicParameters(
                        new
                        {
                            Phone = userPhoneNumber,
                        })).Result;
            return userFromDatabase.Count > 0 ? userFromDatabase[0] : new UserEntity();
        }

        /// <summary>
        /// Add User to database.
        /// </summary>
        /// <param name="userPhoneNumber">
        /// User phone number.
        /// </param>
        /// <param name="userName">
        /// User name.
        /// </param>
        /// <returns>
        /// True, if user is added successfully.
        /// </returns>
        public Task<bool> AddUserToDatabase(string userPhoneNumber, string userName)
        {
            Console.WriteLine("Starting adding user to database");
            bool result = this.SqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.InsertNewUser, new DynamicParameters(
            new
            {
                Phone = userPhoneNumber,
                Name = userName,
            })).IsCompletedSuccessfully;

            if (result)
            {
                Console.WriteLine($"User: {userName} added successfully to database");
                return Task.FromResult(true);
            }
            else
            {
                Console.WriteLine($"There is problem with adding {userName}");
                return Task.FromResult(false);
            }
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
            bool result = this.SqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(
                SqlCommands.AddNewImage,
                new DynamicParameters(
                    new
                    {
                        Path = pathToPicture,
                        ID = userID,
                    })).IsCompletedSuccessfully;

            if (result)
            {
                Console.WriteLine($"User: {userID} added successfully picture {pathToPicture} to database");
                return Task.FromResult(true);
            }
            else
            {
                Console.WriteLine($"There is problem with adding {pathToPicture}");
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Get all pictures entities from database.
        /// </summary>
        /// <returns>
        /// List of picture entities.
        /// </returns>
        public Task<List<PictureEntity>> GetAllPictures()
        {
            List<PictureEntity> pictureEntities = this.SqlServerDataAccess.ExecuteStoredProcedures<PictureEntity>(SqlCommands.GetAllPictures).Result;
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
            Console.WriteLine("Starting adding picture to database");
            bool result = this.SqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(
                SqlCommands.DeletePictureByPath,
                new DynamicParameters(
                    new
                    {
                        Path = pathToPicture,
                    })).IsCompletedSuccessfully;

            if (result)
            {
                Console.WriteLine($"{pathToPicture} deleted from database");
                return Task.FromResult(true);
            }
            else
            {
                Console.WriteLine($"There is problem with deleting {pathToPicture}");
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Check if data is correct.
        /// </summary>
        /// <param name="userToCheck">
        /// User entity from database to check.
        /// </param>
        /// <param name="userPhone">
        /// Current user phone.
        /// </param>
        /// <param name="userName">
        /// Current user name.
        /// </param>
        /// <returns>
        /// Empty string if everything is alright, otherwise contains information about problem.
        /// </returns>
        private Task<string> CheckIfDataIsCorrect(UserEntity userToCheck, string userPhone, string userName)
        {
            string message = string.Empty;
            if (userToCheck.UserName != userName)
            {
                message += "Imię nie zgadza się z podanym numerem telefonu!";
            }

            if (userToCheck.UserPhone != userPhone)
            {
                message += "Numer telefonu się nie zgadza!";
            }

            return Task.FromResult(message);
        }
    }
}
