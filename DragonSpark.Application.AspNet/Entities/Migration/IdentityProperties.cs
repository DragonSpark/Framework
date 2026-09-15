using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class IdentityProperties : ReferenceValueStore<ITypeBase, ImmutableList<IProperty>>
{
	public static IdentityProperties Default { get; } = new();

	IdentityProperties() : base(ComposeIdentityProperties.Default) {}
}