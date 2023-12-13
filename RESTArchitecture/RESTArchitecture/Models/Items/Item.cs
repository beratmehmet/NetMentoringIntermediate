using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RESTArchitecture.Models.Categories;

namespace RESTArchitecture.Models.Items
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
