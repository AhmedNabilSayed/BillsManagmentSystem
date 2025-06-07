﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BillsEntity
{
	public class AppUser : IdentityUser
	{
		public bool IsDeleted { get; set; }
		public string? ImageProfile { get; set; }
		public bool? RememberMe { get; set; }
	}
}
