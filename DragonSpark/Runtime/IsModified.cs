using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime;

sealed class IsModified<T> : InverseCondition<T>
{
	public static IsModified<T> Default { get; } = new IsModified<T>();

	IsModified() : base(IsDefault<T>.Default) {}
}