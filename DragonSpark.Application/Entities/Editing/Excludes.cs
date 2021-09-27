using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace DragonSpark.Application.Entities.Editing
{
	sealed class Excludes : ISelect<DbContext, ExcludeSession>
	{
		public static Excludes Default { get; } = new Excludes();

		Excludes() : this(Excluded.Default) {}

		readonly ISelect<DbContext, Func<EntityEntry, bool>> _predicates;

		public Excludes(ISelect<DbContext, Func<EntityEntry, bool>> predicates) => _predicates = predicates;

		public ExcludeSession Get(DbContext parameter)
			=> new(parameter.ChangeTracker.Entries(), _predicates.Get(parameter));
	}
}