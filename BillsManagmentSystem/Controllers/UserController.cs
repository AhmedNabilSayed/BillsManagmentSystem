using BillsEntity;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace BillsManagmentSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

		public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
		}
        // GET: UserController
        public async Task<ActionResult> Index(string? searchByName)
        {
            IEnumerable<AppUser> users; 
            if (searchByName == null)
                users =await _userManager.Users.ToListAsync();
            else
                users = await _userManager.Users.Where(c => c.UserName.ToLower().Trim().Contains(searchByName.ToLower().Trim())).ToListAsync();

            return View(users);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if(id == null)
                return BadRequest(string.Empty);
            var user =await _userManager.FindByIdAsync(id);
            if(user == null)
				return BadRequest(string.Empty);

			return View(user);
        }

        // GET: UserController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        // POST: UserController/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            try
            {
				if (ModelState.IsValid)
				{
                    var existUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existUser == null)
                    {
						var user = new AppUser()
						{
							Email = model.Email,
							UserName = model.Email.Split('@')[0],
							PhoneNumber = model.PhoneNumber
						};



						var result = await _userManager.CreateAsync(user, model.Password);

						if (result.Succeeded)
						{
							await _userManager.AddToRoleAsync(user, "User");

							return RedirectToAction("Index");
						}
						else
						{
							foreach (var error in result.Errors)
							{
								ModelState.AddModelError(string.Empty, error.Description);
							}
						}
					}
					else
					{
						ModelState.AddModelError("", "Email is already existing");
						return View(model);
					}
                }
					return View(model);


			}
			catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return BadRequest(string.Empty);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
				return BadRequest(string.Empty);

			return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AppUser model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    user.UserName = model.UserName;
                    user.NormalizedUserName = model.UserName.ToUpper();
                    user.PhoneNumber = model.PhoneNumber;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                }
            }
            return View(model);
        }

        // POST: UserController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return BadRequest("User Not Found");
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
    }
}
