using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public sealed class DestinationModelChecker : ISelect<DestinationModelCheckerInput, DestinationModelResult>
{
	public static DestinationModelChecker Default { get; } = new();

	DestinationModelChecker() : this(IsExact.Default) {}

	readonly ICondition<IsExactInput> _exact;

	public DestinationModelChecker(ICondition<IsExactInput> exact) => _exact = exact;

	public DestinationModelResult Get(DestinationModelCheckerInput parameter)
	{
		var (types, destination) = parameter;

		var entities  = destination.GetEntityTypes().ToDictionary(t => t.Name);
		var exact     = new List<IEntityType>();
		var differing = new List<IEntityType>();
		var missing   = new List<IEntityType>();

		foreach (var from in types)
		{
			var collection = entities.TryGetValue(from.Name, out var to)
				                 ? _exact.Get(new(from, to)) ? exact : differing
				                 : missing;
			collection.Add(from);
		}

		return new(new(exact.AsReadOnly(), differing.AsReadOnly()), missing.AsReadOnly());
	}
}