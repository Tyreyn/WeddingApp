namespace WeddingApp.Data.Entities
{
    using WeddingAppDTO.DataTransferObject;

    public class FoodTable
    {
        public string Type { get; set; }

        public ICollection<Food> Food { get; set; }
    }
}
