using DragonSpark.Model.Selection;
using System.ComponentModel;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ChangeType : ISelect<ChangeTypeInput, object?>
{
	public static ChangeType Default { get; } = new();

	ChangeType() {}

	public object? Get(ChangeTypeInput parameter)
	{
		var (value, sourceType, targetType) = parameter;
		try
		{
			return Convert.ChangeType(value, targetType);
		}
		catch
		{
			var targetConverter = TypeDescriptor.GetConverter(targetType);
			if (targetConverter.CanConvertFrom(sourceType))
			{
				return targetConverter.ConvertFrom(value);
			}

			var sourceConverter = TypeDescriptor.GetConverter(sourceType);
			if (sourceConverter.CanConvertTo(targetType))
			{
				return sourceConverter.ConvertTo(value, targetType);
			}
		}

		return null;
	}
}