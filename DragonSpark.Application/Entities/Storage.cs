using Microsoft.EntityFrameworkCore;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Entities
{
	public class Storage<T> : Security.Identity.IdentityDbContext<T> where T : IdentityUser
	{
		readonly IInitializer _initializer;

		protected Storage(DbContextOptions options) : this(Initializer.Default, options) {}

		protected Storage(IInitializer initializer, DbContextOptions options) : base(options)
			=> _initializer = initializer;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			_initializer.Execute(builder);
		}
	}
}
