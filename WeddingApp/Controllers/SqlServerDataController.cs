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

            List<UserEntity> userFromDatabase =
                this.SqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(
                    SqlCommands.SelectUserByPhoneNumber,
                    new DynamicParameters(
                        new
                        {
                            Phone = userPhone,
                        })).Result;

            if (userFromDatabase.Count() == 0)
            {
                return Task.FromResult(new Tuple<bool, string>(
                    false,
                    "User does not exists in database"));
            }

            string message = this.CheckIfDataIsCorrect(userFromDatabase[0], userPhone, userName).Result;
            return Task.FromResult(new Tuple<bool,string>(
                true,
                message));
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
