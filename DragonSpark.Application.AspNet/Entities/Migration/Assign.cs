using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Assign : ICommand<AssignInput>
{
	public static Assign Default { get; } = new();

	Assign() {}

	public void Execute(AssignInput parameter)
	{
		var (from, to) = parameter;

		foreach (var property in from.Properties)
		{
			var name = property.Name;
			if (name is not "Id" && name != to.StructuralType.GetDiscriminatorPropertyName() &&
			    Allow(property, to.Properties, name))
			{
				to[name] = from[name];
			}
		}
	}

	static bool Allow(IProperty source, IReadOnlyList<IProperty> properties, string name)
	{
		for (var i = 0; i < properties.Count; i++)
		{
			var destination = properties[i];
			if (destination.Name == name && destination.ClrType.IsAssignableTo(source.ClrType))
			{
				return true;
			}
		}

		return false;
	}
}