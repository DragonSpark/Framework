using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public class ModelStatus : ISelect<DestinationModelCheckerInput, ModelStatusResult>
{
	readonly IModelTypes                       _types;
	readonly IEntityComparison                 _comparison;

	protected ModelStatus(IModelTypes types) : this(types, EntityComparison.Default) {}

	protected ModelStatus(IModelTypes types, IEntityComparison comparison)
	{
		_types      = types;
		_comparison = comparison;
	}

	public ModelStatusResult Get(DestinationModelCheckerInput parameter)
	{
		var (types, destination) = parameter;

		var locate    = _types.Get(destination);
		var exact     = new List<IEntityType>();
		var differing = new List<ComparisonResult>();
		var missing   = new List<IEntityType>();

		foreach (var from in types)
		{
			var to         = locate.Get(from);
			if (to is not null)
			{
				var comparison = _comparison.Get(new(from, to));
				if (comparison.Changes > 0)
				{
					differing.Add(comparison);
				}
				else
				{
					exact.Add(from);
				}
			}
			else
			{
				missing.Add(from);
			}
		}

		return new(new(exact.AsReadOnly(), differing.AsReadOnly()), missing.AsReadOnly());
	}
}