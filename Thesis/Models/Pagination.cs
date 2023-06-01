namespace Thesis.Models
{
    public class Pagination
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public bool hasPrevious { get; set; }
        public bool hasNext { get; set; }
        public string link { get; set; }

        public Pagination(int currentPage, int totalPages, string link)
        {
            this.currentPage = currentPage;
            this.totalPages = totalPages;
            this.hasPrevious = currentPage > 1;
            hasNext = currentPage < totalPages;
            this.link = link;
        }
    }
}
