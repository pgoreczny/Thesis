namespace Thesis.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public string showName { get; set; }
        public string path { get; set; }
        public enConnectionType bindingType { get; set; }
        public int binding { get; set; }
    }

    public enum enConnectionType
    {
        activity = 0,
        answer = 1,
    }
}
