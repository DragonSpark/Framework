using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class KnownKeys<T> : ReferenceValueStore<DbContext, ImmutableHashSet<object>>, IKnownKeys where T : class
{
	public static KnownKeys<T> Default { get; } = new();

	KnownKeys() : base(ComposeKnownKeys<T>.Default) {}
}

sealed class KnownKeys : ReferenceValueStore<DbContext, ITypeKeys>, ISelect<EntityEntry, ImmutableHashSet<object>>
{
	public static KnownKeys Default { get; } = new();

	KnownKeys() : base(x => new TypeKeys(new ComposeTypeKeys(x).Then().Stores().New())) {}

	public ImmutableHashSet<object> Get(EntityEntry parameter)
		=> Get(parameter.Context).Get(parameter.Metadata.ClrType);
}