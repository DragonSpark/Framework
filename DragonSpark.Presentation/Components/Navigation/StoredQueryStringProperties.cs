using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Presentation.Components.Navigation;

sealed class StoredQueryStringProperties : ArrayStore<Type, QueryStringProperty>
{
	public static StoredQueryStringProperties Default { get; } = new();

	StoredQueryStringProperties() : base(QueryStringProperties.Default) {}
}