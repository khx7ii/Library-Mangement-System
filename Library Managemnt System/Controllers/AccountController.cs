using Library_Managemnt_System.Models;
using Library_Managemnt_System.Repositories;
using Library_Managemnt_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library_Managemnt_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        IBookRepository bookRepository;
        ICheckoutRepository checkoutRepository;
        ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;

        public AccountController
            (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IBookRepository _bookRepository,ICheckoutRepository _checkoutRepository,IUserRepository _userRepository, ICommentRepository _commentRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            bookRepository = _bookRepository;
            checkoutRepository = _checkoutRepository;
            userRepository = _userRepository;
            commentRepository = _commentRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
		[HttpPost]//<a href>
		[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterUser newUserVM)
		{
			if (ModelState.IsValid)
			{
				//create account
				ApplicationUser userModel = new ApplicationUser();
				userModel.UserName = newUserVM.UserName;
				userModel.PasswordHash = newUserVM.Password;
				userModel.PhoneNumber = newUserVM.PhoneNumber;
                userModel.Email = newUserVM.Email;
				IdentityResult result = await userManager.CreateAsync(userModel, newUserVM.Password);
				if (result.Succeeded == true)
				{
					//creat cookie
					await signInManager.SignInAsync(userModel, false);
                    return RedirectToAction("Login", "Account");
                }
				else
				{
					foreach (var item in result.Errors)
					{
						ModelState.AddModelError("", item.Description);
					}
				}

			}
			return View("Register",newUserVM);
		}
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult RoleRegister()
        {
            return View("RegisterRole");
        }
        [HttpPost]//<a href>
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleRegister(RegisterUser newUserVM)
        {
            if (ModelState.IsValid)
            {
                //create account
                ApplicationUser userModel = new ApplicationUser();
                userModel.UserName = newUserVM.UserName;
                userModel.PasswordHash = newUserVM.Password;
                userModel.PhoneNumber = newUserVM.PhoneNumber;
                userModel.Email = newUserVM.Email;
                IdentityResult result = await userManager.CreateAsync(userModel, newUserVM.Password);
                if (result.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(userModel, "Admin");
                    //creat cookie
                    await signInManager.SignInAsync(userModel, false);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View("RegisterRole",newUserVM);
        }

        public IActionResult Login()
        {
            return View("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]//requets.form['_requetss]
       // [Authorize(Roles ="Admin")]
        public async Task<IActionResult> SaveLogin(LoginUser userViewModel)
        {
            if (ModelState.IsValid == true)
            {
                //check found 
                ApplicationUser appUser =
                    await userManager.FindByNameAsync(userViewModel.Name);
                if (appUser != null)
                {
                    bool found =
                         await userManager.CheckPasswordAsync(appUser, userViewModel.Password);
                    if (found == true)
                    {
                        List<Claim> Claims = new List<Claim>();                      

                        await signInManager.SignInWithClaimsAsync(appUser, userViewModel.RememberMe, Claims);
                        //await signInManager.SignInAsync(appUser, userViewModel.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }

                }
                ModelState.AddModelError("", "Username OR Password wrong");
                //create cookie
            }
            return View("Login", userViewModel);
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
        [Authorize]
        public async Task<IActionResult> Borrow(int id)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            var Book =   bookRepository.GetById(id);
            BookViewModel bookViewModel= new BookViewModel();
            bookViewModel.Id = id;
            bookViewModel.Name = Book.Name;
            bookViewModel.Author= Book.Author;
            bookViewModel.Quantity=Book.Quantity;
            bookViewModel.CategoryId=Book.CategoryId;
            bookViewModel.CategoryName = Book.Category.Name;
            bookViewModel.Describtion=Book.Describtion;
            bookViewModel.ImagePath = Book.Image;
            ViewBag.comments=commentRepository.GetCommentsByBook(id);        
            ViewBag.Bookname = Book.Name;
            if (User.Identity.IsAuthenticated)
            {
                Claim UserClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userid = UserClaim.Value;
                applicationUser = await userManager.FindByIdAsync(userid);
                if (applicationUser != null)
                {
                    List<Checkout> checkoutdb = checkoutRepository.GetByBookId(id);
                    foreach (var item in checkoutdb)
                    {
                        if (item.UserId == applicationUser.Id && item.BookId == id&&item.ActiveFlag==true)
                        {
                            ViewBag.Borrowed = "**You have borrowed this book**";
                            return View("Borrow", bookViewModel);
                        }

                    }
                }
            }
            return View(bookViewModel);
        }
        [HttpPost]
        public async Task <IActionResult> SaveBorrow(BookViewModel book)
        {
            ApplicationUser applicationUser = new ApplicationUser();
			Checkout checkout = new Checkout();
            Book bookdb= bookRepository.GetById(book.Id);
            // BookViewModel bookview= new BookViewModel();
            //bookdb.Author = book.Author;
            //bookdb.Name=book.Name;
            //bookdb.Quantity = book.Quantity;
            //bookdb.Describtion = book.Describtion;
            //bookdb.CategoryId= book.CategoryId;

            if (User.Identity.IsAuthenticated)
			{
				Claim UserClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
				string id = UserClaim.Value;
                applicationUser=await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    
					checkout.BookId = book.Id;
					checkout.UserId=applicationUser.Id;
                    checkout.CheckoutDate = DateTime.Now;
                    checkout.DueDate = checkout.CheckoutDate.AddDays(1);  
                    checkout.ActiveFlag = true;
                    bookdb.Quantity -= 1;
                    //applicationUser.Checkouts.Add(checkout);
                    bookRepository.Update(bookdb);
                    checkoutRepository.Add(checkout);
                    checkoutRepository.Save();
                    ViewBag.Success = "The Book has been Borrowed successfully";
                    return RedirectToAction("GetAll", "Category");
				}
				
			}
            return View("Borrow", book);
        }
        public async Task<IActionResult> Return(int id)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            Book bookdb = bookRepository.GetById(id);
            if (User.Identity.IsAuthenticated)
            {
                Claim UserClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string Userid = UserClaim.Value;
                applicationUser = await userManager.FindByIdAsync(Userid);
                if (applicationUser != null)
                {
                    List<Checkout> checkoutdb = checkoutRepository.GetAll();
                    foreach (var item in checkoutdb)
                    {
                        
                        if (item.UserId == applicationUser.Id && item.BookId == id && item.ActiveFlag==true)
                        {
                            int lateDate = DateTime.Now.Day - item.CheckoutDate.Day-1;
                           // ViewBag.lateTax = Math.Max(lateDate, 0) * 5;
                            item.Penalty= Math.Max(lateDate, 0) * 5;
                            item.ReturnDate = DateTime.Now;
                            checkoutRepository.Save();
                            return View("ReturnLateTax", item);
                        }
                    }

                }
            }
            ViewBag.NotBorrowed = "You have not borrowed this book to Return";
            return View("ReturnLateTax");
        }
        [HttpPost]
        public async Task<IActionResult> saveReturn(Checkout checkout)
        {
            checkout = checkoutRepository.GetById(checkout.Id);
            ApplicationUser applicationUser = await userManager.FindByIdAsync(checkout.UserId);
            Book bookdb = bookRepository.GetById(checkout.BookId);            
            bookdb.Quantity += 1;
            bookRepository.Update(bookdb);
            //Checkout userCheckout = applicationUser.Checkouts.FirstOrDefault(checkout);
            //checkoutRepository.Delete(checkout);
            checkout.ActiveFlag = false;
           
           // userCheckout.ReturnDate = ViewBag.ReturnLateTax;
            checkoutRepository.Save();
            return RedirectToAction("GetAll", "Category");
        }
        [Authorize]
        public async Task<IActionResult> GetAllCheckouts()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);
                var checkouts =await userRepository.GetCheckouts(user.Id);

                return View("Checkouts", checkouts);

            }
            return NotFound();

        }
        [Authorize(Roles ="Admin")]
        public IActionResult GetCheckoutsAdmin()
        {
            List<Checkout>checkouts= checkoutRepository.GetAll();            
            return View("CheckoutsAdmin", checkouts);
        }
    }
    

}
