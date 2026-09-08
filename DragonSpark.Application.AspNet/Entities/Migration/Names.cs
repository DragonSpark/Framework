using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Names : ReferenceValueStore<IEntityType, ImmutableHashSet<string>>
{
	public Names(HashSet<string> skip) : base(new ComposeNames(skip)) {}
}