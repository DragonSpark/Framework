using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

		var names = _names.Get(to.Metadata);
		foreach (var property in from.CurrentValues.Properties)
		{
			var name = property.Name;
			if (names.Contains(name))
			{
				var source = ExtractValueFromSource(name);
				if (source is not null)
				{
					to.Property(name).CurrentValue = source;
				}
			}
		}

		switch (to.State)
		{
			case EntityState.Detached:
				to.Context.Add(to.Entity);
				break;
		}

		return;

		object? ExtractValueFromSource(string name)
		{
			var value  = from.CurrentValues[name];
			var result = value is not null ? _value.Get(new(name, value, to)) : null;
			return result;
		}
	}
}