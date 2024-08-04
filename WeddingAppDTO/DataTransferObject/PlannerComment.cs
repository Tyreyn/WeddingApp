using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingAppDTO.DataTransferObject
{
    public class PlannerComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public required string Comment { get; set; }
    }
}
