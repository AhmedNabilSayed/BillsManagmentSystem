using BillsEntity;
using BillsManagmentSystem.ViewModels;
using Demo.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BillsManagmentSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Login(string? returnUrl)
		{
			return View(new SignInViewModel());
		}
		[HttpPost]
		public async Task<IActionResult> Login(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user =await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
					ModelState.AddModelError("", "Email Don't Exist");
				var IsCorrectPassword =await _userManager.CheckPasswordAsync(user, model.Password);

				if(IsCorrectPassword)
				{
					var result = await _signInManager.PasswordSignInAsync(user, model.Password, true ,  false);

					if(result.Succeeded)
						return RedirectToAction("Index", "Home");
				}

			}
				return View(model);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> EditProfile()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);	
			var user = await _userManager.FindByEmailAsync(email);

			var userVM = new UserViewModel()
			{
				Email = user.Email,
				UserName = user.UserName,
				PhoneNumber = user.PhoneNumber,
				ImageProfile = user.ImageProfile
			};

			if (user == null)
				return null;
			return View(userVM);
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile(UserViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user == null)
					return NotFound();
				user.PhoneNumber = model.PhoneNumber;
				user.UserName = model.UserName;

				if (model.Image != null)
					user.ImageProfile = DocumentSettings.UploadFile(model.Image, "Images");

				await _userManager.UpdateAsync(user);

				return RedirectToAction("Index", "Home");
			}
			return View(model);
		}

		[Authorize]
		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
                var email = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                       
                            ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                        
                    }
                }
            }
			
			return View(model);
		}
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction(nameof(Login));

		}
	}
}
