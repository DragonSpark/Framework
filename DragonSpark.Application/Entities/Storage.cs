using Microsoft.EntityFrameworkCore;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Entities
{
	public class Storage<T> : Security.Identity.IdentityDbContext<T> where T : IdentityUser
	{
		readonly IStorageInitializer _initializer;

		protected Storage(DbContextOptions options, IStorageInitializer initializer) : base(options)
			=> _initializer = initializer;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var initialized = _initializer.Get(builder);
			base.OnModelCreating(initialized);
		}
	}
}