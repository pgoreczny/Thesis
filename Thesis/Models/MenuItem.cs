namespace Thesis.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int? ParentId { get; set; }
        public MenuItem? Parent { get; set; }
        public ICollection<MenuItem>children { get; set; }
    }
}
