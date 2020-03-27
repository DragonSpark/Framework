using Microsoft.EntityFrameworkCore;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Entities
{
	public class Storage<T> : Security.Identity.IdentityDbContext<T> where T : IdentityUser
	{
		protected Storage(DbContextOptions options) : base(options) {}
	}
}