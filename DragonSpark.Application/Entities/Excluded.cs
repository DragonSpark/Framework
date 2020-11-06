using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities
{
	sealed class Excluded : ISelect<DbContext, Predicate<EntityEntry>>
	{
		public static Excluded Default { get; } = new Excluded();

		Excluded() : this(Start.A.Selection<EntityEntry>().By.Calling(x => x.Entity)) {}

		readonly DragonSpark.Compose.Model.Selector<EntityEntry, object> _selector;

		public Excluded(DragonSpark.Compose.Model.Selector<EntityEntry, object> selector) => _selector = selector;

		public Predicate<EntityEntry> Get(DbContext parameter)
			=> _selector.Select(new HashSet<object>(parameter.ChangeTracker.Entries()
			                                                 .AsValueEnumerable()
			                                                 .Select(x => x.Entity)
			                                                 .ToArray()).Contains)
			            .Out()
			            .Then()
			            .Inverse()
			            .Get()
			            .Get;
	}
}