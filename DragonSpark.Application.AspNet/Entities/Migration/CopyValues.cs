using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class CopyValues : ICommand<MapInput>
{
	public static CopyValues Default { get; } = new();

	CopyValues() {}

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;

		var values = new Dictionary<string, object?>();

		foreach (var property in from.CurrentValues.Properties)
		{
			var name  = property.Name;
			var value = from.CurrentValues[name];
			values[name] = value is not null ? DetermineValue(name, value, to) : null;
		}

		to.CurrentValues.SetValues(values);
	}

	object DetermineValue(string name, object value, EntityEntry to)
	{
		if (value.GetType().IsEnum)
		{
			var type = to.Metadata.FindProperty(name)?.ClrType;
			if (type is { IsEnum: true })
			{
				return Convert.ChangeType(value, Enum.GetUnderlyingType(type));
			}
		}

		return value;
	}
}