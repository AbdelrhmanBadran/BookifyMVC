using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Core.ViewModels
{
    public class BookFormViewModel
    {

        public int Id { get; set; }
        [MaxLength(500 , ErrorMessage = "Book Name is Too Long (MAX Length is 500 Char.) ")]
        public string Tilte { get; set; } = null!;
        [Display(Name = "Author" )]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }
        [MaxLength(200)]
        public string publisher { get; set; } = null!;
        [Display(Name = " Publishing Date")]
        public DateTime publisherDate { get; set; } = DateTime.Now;
        public IFormFile? Image { get; set; }
        [MaxLength(50 , ErrorMessage = "Book Name is Too Long (MAX Length is 50 Char.) ")]
        public string Hall { get; set; } = null!;
        [Display(Name = "Is available for rental ?")]
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        [Display(Name = " Category")]
        public IList<int> selectedCategories { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; }

    }
}
