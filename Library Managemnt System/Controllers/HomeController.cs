using Library_Managemnt_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Library_Managemnt_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult TestAuth()
        {
            if(User.Identity.IsAuthenticated)
            {
               Claim UserClaim= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string id = UserClaim.Value;
                return Content($"Welcome {User.Identity.Name}, Your ID={id}");
            }
            return Content("welcome guest");
        }
    }
}
