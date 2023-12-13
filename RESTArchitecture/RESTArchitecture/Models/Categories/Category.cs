using RESTArchitecture.Models.Items;

namespace RESTArchitecture.Models.Categories
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Item>? Items { get; set; }
    }
}
