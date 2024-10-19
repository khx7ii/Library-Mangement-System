using Library_Managemnt_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library_Managemnt_System.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddRole()
        {
            return View("AddRole");
        }
        public async Task<IActionResult> SaveRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = roleViewModel.Name;
                IdentityResult result = await roleManager.CreateAsync(role);
                if(result.Succeeded)
                {
                    ViewBag.Success = true;

                    return RedirectToAction("GetAll","Book");
                }
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

            }
            return View("AddRole",roleViewModel);
        }

    }
}
