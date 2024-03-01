namespace WeddingApp.Context
{
    #region Usings

    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// SQL server data access.
    /// </summary>
    public class SqlServerDataAccess
    {
        #region Fields and Constants

        /// <summary>
        /// The configuration properties.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// The proper SQL Server connection string.
        /// </summary>
        private string properConnectionString;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDataAccess"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration properties.
        /// </param>
        public SqlServerDataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.CheckConnectionStatus();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if connection to server can be established.
        /// </summary>
        /// <returns>
        /// True, if connection can be established.
        /// </returns>
        public Task<bool> CheckConnectionStatus()
        {
            var connectionstrings = this.configuration.GetSection("ConnectionStringClass").GetChildren();

            foreach (var connectionstring in connectionstrings)
            {
                try
                {
                    using (SqlConnection myConn = new SqlConnection(connectionstring.Value))
                    {
                        myConn.Open();
                        Console.WriteLine($"{connectionstring.Key} connection can be established");
                        properConnectionString = connectionstring.Value;
                        return Task.FromResult(true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{connectionstring.Key} can't be established" + ex.ToString());
                }
            }

            Console.WriteLine("There is no available SQL Server");
            return Task.FromResult(false);
        }

        #endregion
    }
}
