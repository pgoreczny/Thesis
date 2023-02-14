namespace Thesis.Models.Definitions
{
    public class Definitions
    {
        public static List<Definition> definitions = new List<Definition>
        {
             new Definition
            {
                name = "User",
                addable = true,
                editable = true,
                deletable = true,
                fields = new List<Field>
                {
                    new Field { name = "UserName", editable = true},
                    new Field { name = "Email", editable = true},
                    new Field { name = "", editable = true},
                    new Field { name = "", editable = true},
                }
            },








            new Definition
            {
                name = "template",
                addable = true,
                editable = true,
                deletable = true,
                fields = new List<Field>
                {
                    new Field { name = "", editable = true},
                }
            },
        };
    }
}
