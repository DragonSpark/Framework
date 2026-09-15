using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Allowed : ICondition<AllowedInput>
{
	public static Allowed Default { get; } = new();

	Allowed() {}

	public bool Get(AllowedInput parameter)
	{
		var (property, name, properties) = parameter;
		for (var i = 0; i < properties.Count; i++)
		{
			var destination = properties[i];
			if (destination.Name == name && destination.ClrType.IsAssignableTo(property.ClrType))
			{
				return true;
			}
		}

		return false;
	}
}