using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.State;

sealed class Activities : ReferenceValueStore<IActivityReceiver, Stack<object>>
{
	public static Activities Default { get; } = new();

	Activities() : base(_ => []) {}
}