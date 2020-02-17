using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public class Storage<T> : IdentityDbContext<T> where T : IdentityUser
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
