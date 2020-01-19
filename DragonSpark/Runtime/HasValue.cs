using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;

namespace DragonSpark.Runtime
{
	sealed class HasValue<T> : Condition<T?> where T : struct
	{
		[UsedImplicitly]
		public static HasValue<T> Default { get; } = new HasValue<T>();

		HasValue() : base(x => x.HasValue) {}
	}
}