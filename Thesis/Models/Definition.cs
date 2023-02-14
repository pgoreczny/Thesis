namespace Thesis.Models
{
    public class Definition
    {
        public string name { get; set; }
        public bool addable { get; set; }
        public bool editable { get; set; }
        public bool deletable { get; set; }
        public List<Field> fields { get; set; }
    }

    public class Field
    {
        public string name { get; set; }
        public bool editable { get; set; }
    }
}
