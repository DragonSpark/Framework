using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class CopyValues : ICommand<MapInput>
{
	public static CopyValues Default { get; } = new();

	CopyValues() : this(Names.Default, DetermineValue.Default) {}

	readonly ISelect<IEntityType, ImmutableHashSet<string>> _names;
	readonly ISelect<DetermineValueInput, object?>          _value;

	public CopyValues(ISelect<IEntityType, ImmutableHashSet<string>> names, ISelect<DetermineValueInput, object?> value)
	{
		_names = names;
		_value = value;
	}

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;

		var values     = new Dictionary<string, object?>();
		var names      = _names.Get(to.Metadata);
		var properties = from.CurrentValues.Properties;
		for (byte i = 0; i < properties.Count; i++)
		{
			var name = properties[i].Name;
			if (names.Contains(name))
			{
				var value = from.CurrentValues[name];
				values[name] = value is not null ? _value.Get(new(name, value, to)) : null;
			}
		}

		to.CurrentValues.SetValues(values);
	}
}