using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Entities
{
	public class Storage<T> : Security.Identity.IdentityDbContext<T> where T : IdentityUser
	{
		readonly IStorageInitializer    _initializer;
		readonly ICommand<ModelBuilder> _configure;

		protected Storage(DbContextOptions options, IStorageInitializer initializer)
			: this(options, initializer, EmptyCommand<ModelBuilder>.Default) {}

		protected Storage(DbContextOptions options, IStorageInitializer initializer, ICommand<ModelBuilder> configure)
			: base(options)
		{
			_initializer = initializer;
			_configure   = configure;
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var initialized = _initializer.Get(builder);
			_configure.Execute(initialized);
			base.OnModelCreating(initialized);
		}
	}
}