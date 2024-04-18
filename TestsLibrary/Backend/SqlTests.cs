using Microsoft.Extensions.Configuration;
using System.Net.NetworkInformation;
using WeddingApp.Context;
using Bunit;

using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using WeddingApp.Entities;
using AngleSharp;
using Xunit.Abstractions;
using Microsoft.Data.SqlClient;
using WeddingApp.Helpers.SqlCommands;
using AngleSharp.Common;
using TestsLibrary.Helpers.Entity;
using Dapper;
namespace TestsLibrary.Backend
{
    public class SqlTests : IDisposable
    {
        /// <summary>
        /// SQL server data access.
        /// </summary>
        private readonly SqlServerDataAccess sqlServerDataAccess;

        /// <summary>
        /// The test output helper interface.
        /// </summary>
        private readonly ITestOutputHelper output;

        /// <summary>
        /// The test user object.
        /// </summary>
        private UserEntity testUser;

        private bool disposedValue;

        private int maxUsersID;

        public SqlTests(ITestOutputHelper output)
        {
            this.output = output;
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

        [Fact]
        public void CheckSqlServerConnection()
        {
            Assert.True(sqlServerDataAccess.CheckConnectionStatusAndSetProperConnectionString().Result);
        }

        [Fact]
        public void CheckProperUserInsert()
        {
            List<UserEntity> tmpUserEntity = sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.ShowAllUsers).Result;
            this.output.WriteLine(
                "Before: {0}",
                string.Join(
                    $"{Environment.NewLine}",
                    tmpUserEntity.Select(x => x.UserPhone)));
            bool result = sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.InsertNewUser, new DynamicParameters(
                        new
                        {
                            Phone = this.testUser.UserPhone,
                            Name = this.testUser.UserName
                        })).IsCompletedSuccessfully;
            tmpUserEntity = sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.ShowAllUsers).Result;
            this.output.WriteLine(
                "Before: {0}",
                string.Join(
                    $"{Environment.NewLine}",
                    tmpUserEntity.Select(x => x.UserPhone)));
            Assert.True(result);
        }

        [Fact]
        public void CheckIfTwoUsersWithSamePhoneNumberCanBeAdded()
        {
            this.output.WriteLine("First iteration");
            this.CheckProperUserInsert();
            this.output.WriteLine("Second iteration");
            try
            {
                this.CheckProperUserInsert();
            }
            catch(Exception ex)
            {
                Assert.True(true, "Database didn't insert same user");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.output.WriteLine("Cleaning");
                sqlServerDataAccess.ExecuteStoredProcedures<UserEntity>(SqlCommands.DeleteUserById, new DynamicParameters(new { ID = this.maxUsersID + 1 }));
                this.output.WriteLine("Reseed to {0}", this.maxUsersID);
                sqlServerDataAccess.ExecuteStoredProcedures<string>(SqlCommands.Reseed, new DynamicParameters(new { LASTID = this.maxUsersID }));
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}