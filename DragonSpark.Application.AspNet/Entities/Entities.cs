using DragonSpark.Application.AspNet.Entities.Configuration;
using DragonSpark.Application.AspNet.Entities.Initialization;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public class Entities : DbContext
{
	readonly ICommand<ModelCreating> _configure;

	protected Entities(DbContextOptions options) : this(options, EmptyCommand<ModelCreating>.Default) {}

	protected Entities(DbContextOptions options, ICommand<ModelCreating> configure) : base(options)
		=> _configure = configure;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Setting>();
		_configure.Execute(new(this, builder));
		base.OnModelCreating(builder);
	}
}

public class Entities<T> : IdentityDbContext<T> where T : IdentityUser
{
	readonly ICommand<ModelCreating> _configure;

	protected Entities(DbContextOptions options) : this(options, EmptyCommand<ModelCreating>.Default) {}

	protected Entities(DbContextOptions options, ICommand<ModelCreating> configure) : base(options)
	{
		_configure = configure;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Setting>();
		_configure.Execute(new(this, builder));
		base.OnModelCreating(builder);
	}
}