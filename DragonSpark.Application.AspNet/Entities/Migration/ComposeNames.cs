using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ComposeNames : Select<IEntityType, ImmutableHashSet<string>>
{
	public static ComposeNames Default { get; } = new();

	ComposeNames() : base(x => x.GetProperties()
	                            .Where(y => y.Name != x.GetDiscriminatorPropertyName())
	                            .Select(y => y.Name)
	                            .ToImmutableHashSet()) {}
}