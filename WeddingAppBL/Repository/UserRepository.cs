using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WeddingAppDTO.Context;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppBL.Repository
{
    public class UserRepository
    {
        private WeddingAppUserContext Context { get; set; }

        public UserRepository(WeddingAppUserContext weddingAppUserContext)
        {
            Context = weddingAppUserContext;
        }

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

            User userFromDatabase = this.GetUserEntity(userPhone).Result;

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
        /// Get all users entity from database.
        /// </summary>
        /// <returns>
        /// List of users.
        /// </returns>
        public async Task<List<User>> GetUsers()
        {
            return Task.FromResult(this.Context.Users.ToList()).Result;
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
        public async Task<User> GetUserEntity(string userPhoneNumber)
        {
            User userEntity = Context.Users.Where(user => user.UserPhone.Equals(userPhoneNumber)).FirstOrDefault();

            if (userEntity == null)
            {
                return new User { UserName = null, UserPhone = null };
            }
            else
            {
                return userEntity;
            }
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
            User newUserEntity = new User { UserName = userName, UserPhone = userPhoneNumber };
            this.Context.Users.Add(newUserEntity);
            this.Context.SaveChanges();
            Console.WriteLine($"User: {newUserEntity} added successfully to database");
            return Task.FromResult(true);

        }

        public Task<bool> DeleteUserByPhone(string userPhone)
        {
            Context.ChangeTracker.Clear();
            User userToDelete = this.Context.Users.SingleOrDefault(x => x.UserPhone == userPhone);
            this.Context.Users.Remove(userToDelete);
            this.Context.SaveChanges();
            return Task.FromResult(true);
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
        private Task<string> CheckIfDataIsCorrect(User userToCheck, string userPhone, string userName)
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
