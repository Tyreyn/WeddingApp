﻿using Microsoft.Extensions.Configuration;
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
        private UserDto testUser;

        private UserRepository userOperations;

        private int maxUsersID;

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
            options.UseMySQL(configuration.GetConnectionString("MySQL"));
            options.EnableSensitiveDataLogging();
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            userContext = new WeddingAppUserContext(options.Options);

            userOperations = new UserRepository(userContext);

            testUser = new UserDto { UserPhone = "111111111", UserName = "test user" };
            try
            {
                int tmp = userOperations.GetUsers().Result.LastOrDefault().UserID;
                this.maxUsersID = tmp;
            }catch (Exception ex)
            {
                this.maxUsersID=0;
            }
        }

        [TestCase(true)]
        public void CheckProperUserInsert(bool expectedResult = true)
        {
            List<UserDto> tmpUserEntity = userOperations.GetUsers().Result;
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
            await this.userOperations.DeleteUserById(maxUsersID + 1);
            await userContext.Database.ExecuteSqlAsync($"ALTER TABLE users AUTO_INCREMENT = {this.maxUsersID + 1};");
        }

    }
}