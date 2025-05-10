using DragonSpark.Model.Selection.Stores;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.State;

sealed class Activities : ReferenceValueStore<IActivityReceiver, Stack<object>>
{
	public static Activities Default { get; } = new();

	Activities() : base(_ => []) {}
}