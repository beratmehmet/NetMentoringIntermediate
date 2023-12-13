using System.ComponentModel.DataAnnotations;

namespace RESTArchitecture.Models.Items
{
    public class ItemForCreate
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
