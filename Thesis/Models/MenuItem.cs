namespace Thesis.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public List<MenuItem>children = new List<MenuItem>();
        public int parent { get; set; }
    }
}
