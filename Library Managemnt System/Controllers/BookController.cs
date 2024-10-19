using Library_Managemnt_System.Models;
using Library_Managemnt_System.Repositories;
using Library_Managemnt_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace Library_Managemnt_System.Controllers
{
    
    public class BookController : Controller
    {
        IBookRepository bookRepository;
        ICategoryRepository categoryRepository;
        ICheckoutRepository checkoutRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
		public BookController(IBookRepository _bookRepository, ICategoryRepository _categoryRepository, IWebHostEnvironment _webHostEnvironment, ICheckoutRepository checkoutRepository)
        {
            bookRepository = _bookRepository; categoryRepository = _categoryRepository; webHostEnvironment = _webHostEnvironment;
            this.checkoutRepository = checkoutRepository;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Add()
        {
            ViewData["Categories"]=categoryRepository.GetAll();
            return View("Add");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveAdd(BookViewModel bookrequest)  
        { 
            if(ModelState.IsValid)
            {
				Book book = new Book();
                string uniqueFileName=null;

                //// Save image to "wwwroot/images" folder
                if (bookrequest.Image != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
					 uniqueFileName = Guid.NewGuid().ToString() + "_" + bookrequest.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await bookrequest.Image.CopyToAsync(fileStream);
                    }
                    book.Image = "/Images/" + uniqueFileName;

                }
                //book.Id = bookrequest.Id;
                book.Name = bookrequest.Name;
                book.Author = bookrequest.Author;
                book.CategoryId = bookrequest.CategoryId;
                book.Quantity = bookrequest.Quantity;
                book.Image = "/Images/" + uniqueFileName;
                book.Describtion= bookrequest.Describtion;
                bookRepository.Add(book);
				bookRepository.Save();
				return RedirectToAction("GetAll","Category");
			}
            ViewData["Categories"] = categoryRepository.GetAll();
            return View("Add", bookrequest);
        }
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll(int pageNumber = 1)
        {
            var categories = categoryRepository.GetAll();
            Dictionary<int, string> Categories=new Dictionary<int, string>();
            foreach (var category in categories)
            {
                Categories[category.Id]= category.Name;
            }
            ViewData["Categories"] = Categories;
            var paginateData = bookRepository.GetPaginated(pageNumber);
            ViewData["pageinateData"] = paginateData;

            return View("Books", paginateData.Items);

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id) 
        {
            var book = bookRepository.GetById(id);
            BookViewModel viewModel = new BookViewModel();
			viewModel.Id=book.Id;
            viewModel.Author = book.Author;
            viewModel.CategoryId = book.CategoryId;
            viewModel.Quantity = book.Quantity;
            viewModel.Name = book.Name;
            //viewModel.Image.FileName = book.Image;
            viewModel.Describtion=book.Describtion;
           // ViewBag.img = book.Image.FileName;
            ViewData["Categories"] = categoryRepository.GetAll();
            return View("Edit", viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveUpdate(BookViewModel bookrequest)
        { 
            if (ModelState.IsValid)
            {

				Book BookFromDb = bookRepository.GetById(bookrequest.Id);
                string uniqueFileName = BookFromDb.Image;

                //// Save image to "wwwroot/images" folder
                if (bookrequest.Image != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + bookrequest.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await bookrequest.Image.CopyToAsync(fileStream);
                    }
                    if (!string.IsNullOrEmpty(BookFromDb.Image))
                    {
                        string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, BookFromDb.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    BookFromDb.Image = "/Images/" + uniqueFileName;
                    

                }


                BookFromDb.Name = bookrequest.Name;
                BookFromDb.Author = bookrequest.Author;
                BookFromDb.CategoryId = bookrequest.CategoryId;
                BookFromDb.Quantity = bookrequest.Quantity;
                BookFromDb.Describtion= bookrequest.Describtion;
                bookRepository.Update(BookFromDb);
                bookRepository.Save();
                return RedirectToAction("GetAll", "Category");
            }
            //var deptlist = dbContext.departments.ToList();
            ViewData["Categories"] = categoryRepository.GetAll();
            return View("Edit", bookrequest);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Book BookFromDb = bookRepository.GetById(id);
            bookRepository.Delete(BookFromDb);
            bookRepository.Save();
            return RedirectToAction("GetAll", "Category");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Search(string name)
        {
            name = name.ToLower();
            var bookModel = bookRepository.GetAll().Where(book => book.Name.ToLower().Contains(name) || book.Author.ToLower().Contains(name)).ToList();
            if (name is not null)
            {

                if (bookModel.Count == 0)
                {
                    return View("NotFound");
                }
                var categories = categoryRepository.GetAll();
                Dictionary<int, string> keyValuePairs = new();
                foreach (var item in categories)
                {
                    keyValuePairs[item.Id] = item.Name;
                }

                ViewData["Categories"] = keyValuePairs;
                return View("SearchResult", bookModel);
            }
            return RedirectToAction("GetAll", "Category");
        }







    }
}
