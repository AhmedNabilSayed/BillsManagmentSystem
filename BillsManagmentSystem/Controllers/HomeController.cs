using BillsEntity;
using BillsManagmentSystem.Helper;
using BillsManagmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BillsManagmentSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger , UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);


			GlobalSettings.ImageProfileUrl = user.ImageProfile;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}