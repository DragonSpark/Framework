using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class DetermineValue : ISelect<DetermineValueInput, object?>
{
	public static DetermineValue Default { get; } = new();

	DetermineValue() : this(EnumerationAwareChangeType.Default) {}

	readonly ISelect<ChangeTypeInput, object?> _change;

	public DetermineValue(ISelect<ChangeTypeInput, object?> change) => _change = change;

	public object? Get(DetermineValueInput parameter)
	{
		var (name, value, to) = parameter;
		var property = to.Metadata.FindProperty(name);
		if (property is not null)
		{
			var targetType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
			var sourceType = value.GetType();
			if (sourceType != targetType)
			{
				return _change.Get(new(value, sourceType, targetType));
			}
		}

		return value;
	}
}