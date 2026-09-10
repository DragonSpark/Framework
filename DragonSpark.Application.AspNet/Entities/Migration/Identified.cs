using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Identified : ICommand<IdentifiedInput>
{
	public static Identified Default { get; } = new();

	Identified() : this(IdentityProperties.Default, Allowed.Default) {}

	readonly ISelect<ITypeBase, ImmutableList<IProperty>> _properties;
	readonly ICondition<AllowedInput>                     _allowed;

	public Identified(ISelect<ITypeBase, ImmutableList<IProperty>> properties, ICondition<AllowedInput> allowed)
	{
		_properties = properties;
		_allowed    = allowed;
	}

	public void Execute(IdentifiedInput parameter)
	{
		var (from, to) = parameter;

		var destination = _properties.Get(to.StructuralType);
		foreach (var property in _properties.Get(from.StructuralType))
		{
			var name = property.Name;
			if (_allowed.Get(new(property, name, destination)))
			{
				to[name] = from[name];
			}
		}
	}
}