using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class EnumerationAwareChangeType : ISelect<ChangeTypeInput, object?>
{
	public static EnumerationAwareChangeType Default { get; } = new();

	EnumerationAwareChangeType() : this(ChangeType.Default) {}

	readonly ISelect<ChangeTypeInput, object?> _previous;

	public EnumerationAwareChangeType(ISelect<ChangeTypeInput, object?> previous) => _previous = previous;

	public object? Get(ChangeTypeInput parameter)
	{
		var (value, sourceType, targetType, _) = parameter;
		if (sourceType.IsEnum || targetType.IsEnum)
		{
			if (targetType.IsEnum)
			{
				var underlying = Enum.GetUnderlyingType(targetType);
				try
				{
					var converted = Convert.ChangeType(value, underlying);
					return Enum.ToObject(targetType, converted);
				}
				catch
				{
					// Fall through to general if enum conversion fails
				}
			}
		}

		return _previous.Get(parameter);
	}
}