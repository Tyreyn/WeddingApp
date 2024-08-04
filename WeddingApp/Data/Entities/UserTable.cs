namespace WeddingApp.Data.Entities
{
    using WeddingAppDTO.DataTransferObject;

    public class UserTableClass
    {
        public User userDto { get; set; }

        public bool ShowDetails { get; set; } = false;
    }
}
