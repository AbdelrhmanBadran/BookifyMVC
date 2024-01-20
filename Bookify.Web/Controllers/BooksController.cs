using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper  _mapper;
        private readonly List<string>  IsAllowedExtension = new() {".jpg" , ".jpeg" , ".png"};
        private readonly int  IsAllowedSize = 2097152;
        public BooksController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var ViewModel = PopulateViewModel();
            return View("Form" , ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookFormViewModel model)
        {
            var extension = Path.GetExtension(model.Image?.FileName);

            if (!ModelState.IsValid)
            {
                model = PopulateViewModel(model);
                return View("Form", model);
            }

            var book = _mapper.Map<Book>(model);

            if(model.Image is not null)
            {
                if(!IsAllowedExtension.Contains(extension!))
                {
                    ModelState.AddModelError(nameof(model.Image), "Extension of this file is not available! only allowed ( .jpg , .jpeg , .png )");
                    return View("Form", model);
                }

                if(IsAllowedSize > model.Image.Length)
                {
                    ModelState.AddModelError(nameof(model.Image), "Size of image is bigger than 2MB");
                    return View("Form", model);

                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", imageName);
                
                using var stream = System.IO.File.Create(path);
                
                model.Image.CopyTo(stream);

                book.ImageUrl = imageName;
            }
            foreach (var category in model.selectedCategories)
                book.Categories.Add(new BookCategory { CategoryId = category });


            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        private BookFormViewModel PopulateViewModel(BookFormViewModel? model = null)
        {
            var viewModel = model is null ? new BookFormViewModel() : model;

            var authors = _dbContext.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = _dbContext.Categories.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();

            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);

            return viewModel; 

        }
    }   
}
