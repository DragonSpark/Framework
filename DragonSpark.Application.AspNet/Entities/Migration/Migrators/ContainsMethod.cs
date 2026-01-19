using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ContainsMethod : Instance<MethodInfo>
{
	public static ContainsMethod Default { get; } = new();

	ContainsMethod() : base(typeof(ImmutableHashSet<object>).GetMethod(nameof(ImmutableHashSet<>.Contains)).Verify()) {}
}