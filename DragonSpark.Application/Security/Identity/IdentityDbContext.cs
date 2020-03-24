using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Security.Identity
{
	public class IdentityDbContext<T>
		: Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<T, IdentityRole<int>, int>
		where T : IdentityUser
	{
		public IdentityDbContext() {}

		public IdentityDbContext(DbContextOptions options) : base(options) {}
	}
}