namespace WeddingApp.Helpers.SqlCommands
{

    /// <summary>
    /// SQL commands storage.
    /// </summary>
    public static class SqlCommands
    {
        /// <summary>
        /// Insert new user procedure name
        /// Parameters: (@Phone = user phone number,@Name = user name).
        /// </summary>
        public static readonly string InsertNewUser = "WeddingDB.dbo.INSERT_NEW_USER";

        /// <summary>
        /// Delete user by id procedure name.
        /// Parameters: (@ID = user id).
        /// </summary>
        public static readonly string DeleteUserById = "WeddingDB.dbo.DELETE_USER_BY_ID";

        /// <summary>
        /// Delete user by phone number procedure name.
        /// Parameters: (@Phone = user phone number).
        /// </summary>
        public static readonly string DeleteUserByPhoneNumber = "WeddingDB.dbo.DELETE_USER_BY_PHONE_NUMBER";

        /// <summary>
        /// Show all users procedure name.
        /// </summary>
        public static readonly string ShowAllUsers = "WeddingDB.dbo.SHOW_ALL_USERS";

        /// <summary>
        /// Reseed procedure name.
        /// </summary>
        public static readonly string Reseed = "WeddingDB.dbo.RESEED";

        /// <summary>
        /// Get max user id procedure name.
        /// </summary>
        public static readonly string GetMaxUserId = "WeddingDB.dbo.GET_MAX_ID";

        /// <summary>
        /// Select user by phone number procedure name.
        /// Parameters: (@Phone = user phone number).
        /// </summary>
        public static readonly string SelectUserByPhoneNumber = "WeddingDB.dbo.SELECT_USER_BY_PHONE_NUMBER";

        /// <summary>
        /// Select user by name procedure name.
        /// Parameters: (@Name = user name).
        /// </summary>
        public static readonly string SelectUserByName = "WeddingDB.dbo.SELECT_USER_BY_Name";

        /// <summary>
        /// Add new picture procedure name.
        /// Parameters: (@Path = path to picture, @ID = user ID).
        /// </summary>
        public static readonly string AddNewImage = "WeddingDB.dbo.ADD_NEW_IMAGE";

        /// <summary>
        /// Get all added pictures procedure name.
        /// </summary>
        public static readonly string GetAllPictures = "WeddingDB.dbo.SHOW_ALL_PICTURES";

        /// <summary>
        /// Delete picture by path procedure name.
        /// Parameters: (@Path = path to picture).
        /// </summary>
        public static readonly string DeletePictureByPath = "WeddingDB.dbo.DELETE_PICTURE_BY_PATH";
    }
}
