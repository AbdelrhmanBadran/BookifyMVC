namespace Bookify.Web.Core.Models
{
     [Index(nameof(Tilte) , nameof(AuthorId) , IsUnique = true)]
    public class Book
    {
        public int Id{ get; set; }
        [MaxLength(500)]
        public string Tilte { get; set; } = null!;
        public int AuthorId{ get; set; }
        public Author Author{ get; set; }
        [MaxLength(200)]
        public string publisher { get; set; } = null!;
        public DateTime publisherDate{ get; set; }
        public string? ImageUrl{ get; set; }
        [MaxLength(50)]
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public ICollection<BookCategory> Categories{ get; set;} = new List<BookCategory>();
    }
}
