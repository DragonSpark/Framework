using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Navigation;

sealed class IsOn : ICondition<IsOnInput>
{
	public static IsOn Default { get; } = new();

	IsOn() : this(StringComparison.InvariantCultureIgnoreCase) {}

	readonly StringComparison _comparison;

	public IsOn(StringComparison comparison) => _comparison = comparison;

	public bool Get(IsOnInput parameter)
	{
		var (subject, path) = parameter;
		var result = subject.Path().StartsWith(path.TrimStart('/'), _comparison);
		return result;
	}
}