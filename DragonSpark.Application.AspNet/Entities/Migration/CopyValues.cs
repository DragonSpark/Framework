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

	CopyValues() : this(AssignValue.Default) {}

	readonly ISelect<IEntityType, ImmutableHashSet<string>> _names;
	readonly ISelect<DetermineValueInput, object?>          _value;
	readonly IAssignValue                                   _assign;

	public CopyValues(IAssignValue assign) : this(Names.Default, DetermineValue.Default, assign) {}

	public CopyValues(ISelect<IEntityType, ImmutableHashSet<string>> names, ISelect<DetermineValueInput, object?> value,
	                  IAssignValue assign)
	{
		_names  = names;
		_value  = value;
		_assign = assign;
	}

	public void Execute(MapInput parameter)
	{
		var (from, to) = parameter;

		var names = _names.Get(to.Metadata);
		foreach (var property in from.CurrentValues.Properties)
		{
			var name = property.Name;
			if (names.Contains(name) && from.CurrentValues[name] is {} value 
			                         && _value.Get(new(name, value, to)) is {} source)
			{
				_assign.Execute(new(source, to.Property(name)));
			}
		}

		switch (to.State)
		{
			case EntityState.Detached:
				to.Context.Add(to.Entity);
				break;
		}
	}
}