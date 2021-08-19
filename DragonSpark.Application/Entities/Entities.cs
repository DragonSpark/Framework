using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	public class Entities<T> : Security.Identity.IdentityDbContext<T> where T : Security.Identity.IdentityUser
	{
		readonly Alter<ModelBuilder> _select;

		protected Entities(DbContextOptions options, ISchemaModification initializer)
			: this(options, initializer, EmptyCommand<ModelBuilder>.Default) {}

		protected Entities(DbContextOptions options, ISchemaModification initializer,
		                          ICommand<ModelBuilder> configure)
			: this(options, initializer.Then().Configure(configure).Out().Get) {}

		protected Entities(DbContextOptions options, Alter<ModelBuilder> select) : base(options)
			=> _select = select;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var select = _select(builder);
			base.OnModelCreating(select);
		}
	}

	public class Entities : DbContext
	{
		readonly Alter<ModelBuilder> _select;

		protected Entities(DbContextOptions options, ISchemaModification initializer)
			: this(options, initializer, EmptyCommand<ModelBuilder>.Default) {}

		protected Entities(DbContextOptions options, ISchemaModification initializer,
		                ICommand<ModelBuilder> configure)
			: this(options, initializer.Then().Configure(configure).Out().Get) {}

		protected Entities(DbContextOptions options, Alter<ModelBuilder> select) : base(options)
			=> _select = select;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var select = _select(builder);
			base.OnModelCreating(select);
		}
	}

}