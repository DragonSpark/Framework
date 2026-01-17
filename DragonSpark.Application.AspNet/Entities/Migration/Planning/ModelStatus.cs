using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public class ModelStatus : ISelect<ModelStatusInput, ModelStatusResult>
{
	readonly IModelTypes _types;

	protected ModelStatus(IModelTypes types) => _types = types;

	public ModelStatusResult Get(ModelStatusInput parameter)
	{
		var (types, destination) = parameter;

		var locate    = _types.Get(destination);
		var exact     = new List<IEntityType>();
		var differing = new List<EntityComparisonResult>();
		var missing   = new List<IEntityType>();
		var entities  = new EntityComparison(locate);

		foreach (var from in types)
		{
			var to = locate.Get(from);
			if (to is not null)
			{
				var comparison = entities.Get(new(from, to));
				if (comparison.Modifications.Changes > 0)
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

		differing.Sort((x, y) => x.Modifications.Changes.CompareTo(y.Modifications.Changes));
		return new(new(exact.AsReadOnly(), differing.AsReadOnly()), missing.AsReadOnly());
	}
}