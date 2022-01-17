using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JobsFinds.Data;
using Microsoft.AspNetCore.Mvc;
using JobsFinds.Models;

namespace JobsFinds.Controllers
{
    [Authorize(Roles = "Company")]
    public class CompanyController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;
        private User? user = null;
        public CompanyController(UserManager<User> userManager, AppDBContext context, IWebHostEnvironment env)
        {
            _env = env;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect("/");
            if(user.Type != UserType.Company)
            {
                return Redirect("/");
            }
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Name)
        {
            user = await _userManager.GetUserAsync(User);
            if (user == null) return Redirect("/");
            user.Name = Name;
            var image = HttpContext.Request.Form.Files.GetFile("image");
            var uploads = Path.Combine(_env.WebRootPath, "images");
            if (image != null)
            {
                var fileName = "users/" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(image.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                    user.image = fileName;
                }
            }
            else
            {
                user.image = "users/default_profile.png";
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }

}
