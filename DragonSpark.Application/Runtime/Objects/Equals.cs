using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Runtime.Objects;

public sealed class Equals<T> : ICondition<EqualInput<T>>
{
	public static Equals<T> Default { get; } = new();

	Equals() : this(EqualityComparer<T>.Default) {}

	readonly IEqualityComparer<T> _comparer;

	public Equals(IEqualityComparer<T> comparer) => _comparer = comparer;

	public bool Get(EqualInput<T> parameter)
	{
		var (left, right) = parameter;
		var result = ReferenceEquals(left, right) ||
		             (left is null && right is null) || 
		             (left is IEquatable<T> equatable ? equatable.Equals(right) : _comparer.Equals(left, right));
		return result;
	}
}