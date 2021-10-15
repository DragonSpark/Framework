using DragonSpark.Application.Entities.Initialization;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class Entities : DbContext
{
	readonly ICommand<ModelCreating> _configure;

	protected Entities(DbContextOptions options) : this(options, EmptyCommand<ModelCreating>.Default) {}

	protected Entities(DbContextOptions options, ICommand<ModelCreating> configure) : base(options)
		=> _configure = configure;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		_configure.Execute(new (this, builder));
		base.OnModelCreating(builder);
	}
}

public class Entities<T> : Security.Identity.IdentityDbContext<T> where T : Security.Identity.IdentityUser
{
	readonly ICommand<ModelCreating> _configure;

	protected Entities(DbContextOptions options) : this(options, EmptyCommand<ModelCreating>.Default) {}

	protected Entities(DbContextOptions options, ICommand<ModelCreating> configure) : base(options)
	{
		_configure = configure;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		_configure.Execute(new ModelCreating(this, builder));
		base.OnModelCreating(builder);
	}
}