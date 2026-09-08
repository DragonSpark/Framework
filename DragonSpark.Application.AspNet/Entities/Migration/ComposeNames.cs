using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ComposeNames : Select<IEntityType, ImmutableHashSet<string>>
{
	public static ComposeNames Default { get; } = new();

	ComposeNames() : this([]) {}

	public ComposeNames(HashSet<string> skip)
		: base(x =>
		[
			.. x.GetProperties()
			    .Where(y => y.Name != x.GetDiscriminatorPropertyName() && !skip.Contains(y.Name))
			    .Select(y => y.Name)
		]) {}
}