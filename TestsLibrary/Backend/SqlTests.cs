using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using System;
using WeddingApp.Helpers.SqlCommands;
using TestsLibrary.Helpers.Entity;

namespace TestsLibrary.Backend
{
    [TestFixture]
    public class SqlTests
    {
        /// <summary>
        /// SQL server data access.
        /// </summary>
        private SqlServerDataAccess sqlServerDataAccess;

        /// <summary>
        /// The test user object.
        /// </summary>
        private UserEntity testUser;


        private int maxUsersID;


        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<ConnectionStringClass>()
                .AddEnvironmentVariables()
                .Build();
            testUser = new UserEntity();
            testUser.UserPhone = "111111111";
            testUser.UserName = "test user";
            sqlServerDataAccess = new SqlServerDataAccess(configuration);
            List<MaxId> tmp = sqlServerDataAccess.ExecuteStoredProcedures<MaxId>(SqlCommands.GetMaxUserId).Result;
            this.maxUsersID = tmp[0].MAX;
        }

        [Test]
        public void CheckSqlServerConnection()
        {
            Assert.That(sqlServerDataAccess.CheckConnectionStatusAndSetProperConnectionString().Result);
        }

        [TestCase(true)]
        public void CheckProperUserInsert(bool expectedResult = true)
        {
            List<UserEntity> tmpUserEntity = sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.ShowAllUsers).Result;
            bool result = sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.InsertNewUser, new DynamicParameters(
                        new
                        {
                            Phone = this.testUser.UserPhone,
                            Name = this.testUser.UserName
                        })).IsCompletedSuccessfully;
            tmpUserEntity = sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.ShowAllUsers).Result;
            Assert.That(result == expectedResult);
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
        public void TearDown()
        {
            sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.DeleteUserById, new DynamicParameters(new { ID = this.maxUsersID + 1 }));
            sqlServerDataAccess.ExecuteStoredProcedures<string>(SqlCommands.Reseed, new DynamicParameters(new { LASTID = this.maxUsersID }));

        }

    }
}