using DragonSpark.Model.Selection;
using System;
using System.ComponentModel;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ChangeType : ISelect<ChangeTypeInput, object?>
{
	public static ChangeType Default { get; } = new();

	ChangeType() {}

	public object? Get(ChangeTypeInput parameter)
	{
		var (value, sourceType, targetType, name) = parameter;
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

			// Loud fail — better to explode early in migration than corrupt data
			throw new
				InvalidOperationException($"Cannot convert value '{value}' ({sourceType}) to target type {targetType} for property '{name}'.");
		}
	}
}