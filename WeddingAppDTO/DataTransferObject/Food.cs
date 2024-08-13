using System.ComponentModel.DataAnnotations;

namespace WeddingAppDTO.DataTransferObject
{
    public class Food
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Type { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
