using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime
{
	sealed class IsDefault<T> : EqualityCondition<T>
	{
		public static IsDefault<T> Default { get; } = new IsDefault<T>();

		IsDefault() : base(default) {}
	}
}