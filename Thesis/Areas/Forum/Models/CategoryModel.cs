namespace Thesis.Areas.Forum.Models
{
    public class CategoryModel
    {
        public string name {  get; set; }
        public int posts { get; set; }
        public int categoryId { get; set; }
        public string newestActivity { get; set; }
    }
}
