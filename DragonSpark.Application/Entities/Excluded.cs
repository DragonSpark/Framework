using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities
{
	sealed class Excluded : ISelect<DbContext, Func<EntityEntry, bool>>
	{
		public static Excluded Default { get; } = new Excluded();

		Excluded() : this(Start.A.Selection<EntityEntry>().By.Calling(x => x.Entity)) {}

		readonly DragonSpark.Compose.Model.Selection.Selector<EntityEntry, object> _selector;

		public Excluded(DragonSpark.Compose.Model.Selection.Selector<EntityEntry, object> selector) => _selector = selector;

		public Func<EntityEntry, bool> Get(DbContext parameter)
			=> _selector.Select(parameter.ChangeTracker.Entries()
			                             .AsValueEnumerable()
			                             .Select(x => x.Entity)
			                             .ToArray()
			                             .Contains)
			            .Out()
			            .Then()
			            .Inverse()
			            .Get()
			            .Get;
	}
}