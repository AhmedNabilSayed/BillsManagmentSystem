using BillsEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsDAL.Identity
{
	public class AppIdentityDbContext : IdentityDbContext<AppUser>
	{
		public AppIdentityDbContext(DbContextOptions options) : base(options)
		{
		}
	}
}
