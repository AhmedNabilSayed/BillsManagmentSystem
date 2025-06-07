using BillsEntity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsDAL.Identity
{
	public static class AppIdentityDbContextSeed
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if(!userManager.Users.Any())
			{
				var user = new AppUser()
				{
					UserName = "Ahmed_Nabil",
					Email = "ahmed29@gmail.com",
					RememberMe = true,
				};

				await userManager.CreateAsync(user, "An123@");
				await userManager.AddToRoleAsync(user, "Admin");
			}
		}

		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if(!roleManager.Roles.Any())
			{
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
		}
	}
}
