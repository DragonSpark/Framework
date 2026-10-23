using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

sealed class SourceQueries<T> : ReferenceValueStore<IModel, IAlteration<IQueryable<T>>>
{
	public static SourceQueries<T> Default { get; } = new();

	SourceQueries() : base(x => A.Selection(new SourceQuery<T>(x).Then().Stores().New()).Then().Out()) {}
} 