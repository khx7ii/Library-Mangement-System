using Library_Managemnt_System.Models;
using Library_Managemnt_System.Repositories;
using Library_Managemnt_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Library_Managemnt_System.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepository categoryRepository;
        IBookRepository bookRepository; 
        public CategoryController (IBookRepository _bookRepository, ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository; 
            bookRepository = _bookRepository;   
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {

            return View("Add"); 
        }

        [HttpPost]//<a href>
        [Authorize(Roles = "Admin")]
        public IActionResult SaveNew(string Name)
        {
            Category categoryModel = new Category();
            if (ModelState.IsValid)
            {
                
                categoryModel.Name = Name;
                categoryRepository.Add(categoryModel);
                categoryRepository.Save();
                return RedirectToAction("GetAll");   

            }

            return View("Add", categoryModel); 
        }

        public IActionResult GetAll(int PageNumber =1 )
        {

            List<Book> books = bookRepository.GetAll();

            Dictionary<string, List<Book>> dicOfBooks = new();

            foreach (var book in books)
            {
                // 
                if (dicOfBooks.ContainsKey(book.Category.Name))
                {
                    dicOfBooks[book.Category.Name].Add(book); 
                }
                else
                {
                    dicOfBooks[book.Category.Name] = new List<Book>() { book};
                }


            }

            ViewData["booksForEachCategory"] = dicOfBooks;

            var PaginatedData = categoryRepository.GetPaginated(PageNumber);
            
            ViewData["Pagination"] = PaginatedData; 
            return View("GetAll", PaginatedData.Items) ;
        }


        [HttpPost]//<a href>
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.GetById(id);
            categoryRepository.Delete(category);
            categoryRepository.Save(); 
            return RedirectToAction("GetAll");
        }
        [HttpGet]//<a href>
        [Authorize(Roles = "Admin")]

        public IActionResult Edit(int id)
        {
            var categoryModel = categoryRepository.GetById(id);

            return View("Edit", categoryModel);

        }

        [HttpPost]//<a href>
        [Authorize(Roles = "Admin")]
        public IActionResult SaveEdit(int id, Category category)
        {
            var cateoryModel = categoryRepository.GetById(id);

            if (ModelState.IsValid)
            {


                cateoryModel.Name = category.Name;

                categoryRepository.Update(cateoryModel);
                categoryRepository.Save();
                return RedirectToAction("GetAll");

            }
            return View("Edit", cateoryModel);
        }
        [Authorize(Roles = "Admin")]

        public PaginatedListViewModel<Category> Paginated(int pageNumber, List<Category> categories)
        {
            int pageSize = 2;
            var data = categories.OrderBy(c => c.Name)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            int totalItems = categories.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PaginatedListViewModel<Category>
            {
                Items = data,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };
        }



        public IActionResult Search(searchViewModel result)
        {
            if (result.name != null)
            {
                string name = result.name.ToLower();
                int PageNumber = result.PageNumber;
                ViewBag.name = name;

                if (name is not null)
                {
                    List<Book> books = bookRepository.GetAll();

                    Dictionary<string, List<Book>> dicOfBooks = new();
                    foreach (var book in books)
                    {
                        if (book.Name.ToLower().Contains(name) || book.Author.ToLower().Contains(name) || book.Category.Name.ToLower().Contains(name))
                        {
                            if (dicOfBooks.ContainsKey(book.Category.Name.ToLower()))
                            {
                                dicOfBooks[book.Category.Name.ToLower()].Add(book);
                            }
                            else
                            {
                                dicOfBooks[book.Category.Name.ToLower()] = new List<Book>() { book };
                            }
                        }
                    }
                    ViewData["booksForEachCategory"] = dicOfBooks;
                    Dictionary<string, bool> flag = new Dictionary<string, bool>();

                    List<Category> categories = new List<Category>();
                    var categoryModel = categoryRepository.GetAll().ToList();
                    foreach (var category in categoryModel)
                        flag[category.Name.ToLower()] = true;
                    foreach (var category in categoryModel)
                    {
                        if ((dicOfBooks.ContainsKey(category.Name.ToLower()) || category.Name.ToLower().Contains(name)) && flag[category.Name.ToLower()] == true)
                        {

                            categories.Add(category);
                            flag[category.Name.ToLower()] = false;
                        }

                    }
                    if (categories.Count == 0)
                    {
                        return View("NotFound");
                    }

                    var PaginatedData = Paginated(PageNumber, categories);

                    ViewData["Pagination"] = PaginatedData;

                    return View("SearchResult", PaginatedData.Items);

                }
            }

            

            return RedirectToAction("GetAll"); 

        }
    }
}
