using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Presentation.Components
{
	public sealed class ComponentIdentifier : ReferenceValueStore<object, string>
	{
		public static ComponentIdentifier Default { get; } = new ComponentIdentifier();

		ComponentIdentifier() : base(_ => UniqueIdentifier.Default.Get(Guid.NewGuid())) {}
	}
}