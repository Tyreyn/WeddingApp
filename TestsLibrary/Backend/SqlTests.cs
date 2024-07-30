using Microsoft.Extensions.Configuration;
using WeddingApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using WeddingAppDTO.DataTransferObject;
using WeddingAppBL.Repository;
using WeddingAppDTO.Context;

namespace TestsLibrary.Backend
{
    [TestFixture]
    public class SqlTests
    {

        /// <summary>
        /// The test user object.
        /// </summary>
        private User testUser;

        private UserRepository userOperations;

        private WeddingAppUserContext userContext;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<ConnectionStringClass>()
                .AddEnvironmentVariables()
                .Build();

            var options = new DbContextOptionsBuilder<WeddingAppUserContext>();
            options.UseSqlServer(configuration.GetConnectionString("MySQL"));
            options.EnableSensitiveDataLogging();
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            userContext = new WeddingAppUserContext(options.Options);

            userOperations = new UserRepository(userContext);

            testUser = new User { UserPhone = "111111111", UserName = "test user" };
        }

        [TestCase(true)]
        public void CheckProperUserInsert(bool expectedResult = true)
        {
            List<User> tmpUserEntity = userOperations.GetUsers().Result;
            int before = tmpUserEntity.Count;
            this.userOperations.AddUserToDatabase(testUser.UserPhone, testUser.UserName);
            tmpUserEntity = userOperations.GetUsers().Result;
            int after = tmpUserEntity.Count;
            Assert.That((after > before) == expectedResult);
        }

        [Test]
        public void CheckIfTwoUsersWithSamePhoneNumberCanBeAdded()
        {
            this.CheckProperUserInsert();
            try
            {
                this.CheckProperUserInsert(expectedResult: false);
            }
            catch (Exception ex)
            {
                Assert.That(true, "Database didn't insert same user");
            }
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            await this.userOperations.DeleteUserByPhone(testUser.UserPhone);
            //await userContext.Database.ExecuteSqlAsync($"ALTER TABLE users AUTO_INCREMENT = {this.maxUsersID + 1};");
        }

    }
}