using DragonSpark.Model.Selection.Conditions;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class IsExact : ICondition<IsExactInput>
{
	public static IsExact Default { get; } = new();

	IsExact() {}

	public bool Get(IsExactInput parameter)
	{
		var (source, destination) = parameter;

		var properties = source.GetProperties()
		                       .Select(p => p.Name)
		                       .ToHashSet()
		                       .SetEquals(destination.GetProperties().Select(p => p.Name).ToHashSet());
		return properties && source.GetNavigations()
		                           .Select(n => n.Name)
		                           .ToHashSet()
		                           .SetEquals(destination.GetNavigations().Select(n => n.Name).ToHashSet());
	}
}