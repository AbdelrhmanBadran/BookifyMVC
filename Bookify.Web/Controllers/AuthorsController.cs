﻿namespace Bookify.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var authors = _context.Authors.AsNoTracking().ToList();

            var ViewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(authors);

            return View(ViewModel);
        }
        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {

            return PartialView("_Form");
        }
        [HttpPost]
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            var author = _mapper.Map<Author>(model);

            _context.Authors.Add(author);
            _context.SaveChanges();

            var viewModel = _mapper.Map<AuthorViewModel>(author);

            return PartialView("_AuthorRow", viewModel);

        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);

            if (author is null)
                return NotFound();

            var ViewModel = _mapper.Map<AuthorFormViewModel>(author);

            return PartialView("_Form", ViewModel);
        }

        [HttpPost]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var author = _context.Authors.Find(model.Id);

            if (author is null)
                return NotFound();
            author = _mapper.Map(model, author);
            author.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            var viewModel = _mapper.Map<AuthorViewModel>(author);

            return PartialView("_AuthorRow", viewModel);

        }
        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var author = _context.Authors.Find(id);

            if (author is null)
                return NotFound();

            author.IsDeleted = !author.IsDeleted;
            author.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            return Ok(author.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItems(AuthorFormViewModel model)
        {
            var authors = _context.Categories.SingleOrDefault(a => a.Name == model.Name);

            var IsAllowed = authors is null || authors.Id.Equals(model.Id);

            return Json(IsAllowed);
        }


    }
}
