using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class CopyValues : ICommand<MapInput>
{
	public static CopyValues Default { get; } = new();

	CopyValues() : this(DetermineValue.Default) {}

	readonly ISelect<DetermineValueInput, object?> _value;

	public CopyValues(ISelect<DetermineValueInput, object?> value) => _value = value;

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;

		var values = new Dictionary<string, object?>();

		foreach (var property in from.CurrentValues.Properties)
		{
			var name  = property.Name;
			var value = from.CurrentValues[name];
			values[name] = value is not null ? _value.Get(new(name, value, to)) : null;
		}

		to.CurrentValues.SetValues(values);
	}
}

// TODO

public readonly record struct DetermineValueInput(string Name, object Value, EntityEntry To);

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
				return _change.Get(new(value, sourceType, targetType, name));
			}
		}

		return value;
	}
}

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

public readonly record struct ChangeTypeInput(object Value, Type SourceType, Type TargetType, string PropertyName);

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