using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components
{
	public sealed class ComponentIdentifier : ReferenceValueStore<object, string>
	{
		public static ComponentIdentifier Default { get; } = new();

		ComponentIdentifier() : base(UniqueIdentifiers.Default.Then().Any()) {}
	}
}