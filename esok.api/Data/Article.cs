using System.ComponentModel.DataAnnotations;

namespace esok.api.Data
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string PostContent { get; set; }
        public string PagePostId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
    }
}