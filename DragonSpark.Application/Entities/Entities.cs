using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities
{
	public class Entities<T> : Security.Identity.IdentityDbContext<T> where T : Security.Identity.IdentityUser
	{
		readonly Func<SchemaInput, ModelBuilder> _select;

		protected Entities(DbContextOptions options, ISchemaModification initializer)
			: this(options, initializer, EmptyCommand<ModelBuilder>.Default) {}

		protected Entities(DbContextOptions options, ISchemaModification initializer, ICommand<ModelBuilder> configure)
			: this(options, initializer.Then().Configure(configure)) {}

		protected Entities(DbContextOptions options, Func<SchemaInput, ModelBuilder> select) : base(options)
			=> _select = select;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var select = _select(new(GetType(), builder));
			base.OnModelCreating(select);
		}
	}

	public class Entities : DbContext
	{
		readonly Func<SchemaInput, ModelBuilder> _select;

		protected Entities(DbContextOptions options, ISchemaModification initializer)
			: this(options, initializer, EmptyCommand<ModelBuilder>.Default) {}

		protected Entities(DbContextOptions options, ISchemaModification initializer,
		                   ICommand<ModelBuilder> configure)
			: this(options, initializer.Then().Configure(configure)) {}

		protected Entities(DbContextOptions options, Func<SchemaInput, ModelBuilder> select) : base(options)
			=> _select = select;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			var select = _select(new(GetType(), builder));
			base.OnModelCreating(select);
		}
	}
}