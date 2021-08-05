using Microsoft.AspNetCore.Identity;
using System;

namespace DragonSpark.Application.Security.Identity
{
	public class IdentityUser : IdentityUser<int>
	{
		public virtual DateTimeOffset Created { get; set; }

		public virtual DateTimeOffset? Modified { get; set; }
	}
}