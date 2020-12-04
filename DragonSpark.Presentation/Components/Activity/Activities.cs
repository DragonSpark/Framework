using DragonSpark.Model.Selection.Stores;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.Activity
{
	sealed class Activities : ReferenceValueStore<object, ConcurrentBag<object>>
	{
		public static Activities Default { get; } = new Activities();

		Activities() : base(_ => new ConcurrentBag<object>()) {}
	}
}