using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class NamedModels : ICondition<IEntityType>
{
	public static NamedModels Default { get; } = new();

	NamedModels() : this(typeof(Dictionary<,>)) {}

	readonly ISelect<IModel, ImmutableHashSet<IEntityType>> _select;
	
	public NamedModels(Type contact)
		: this(Start.A.Selection<IModel>()
		            .By.Calling(x => x.GetEntityTypes()
		                              .Where(y => y.ClrType.IsGenericType &&
		                                          y.ClrType.GetGenericTypeDefinition() == contact)
		                              .ToImmutableHashSet())
		            .Stores()
		            .New()) {}

	public NamedModels(ISelect<IModel, ImmutableHashSet<IEntityType>> select) => _select = select;

	public bool Get(IEntityType parameter) => _select.Get(parameter.Model).Contains(parameter);
}