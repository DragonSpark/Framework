using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class Keys : ISelect<EntityEntry, object>
{
	public static Keys Default { get; } = new();

	Keys() {}

	public object Get(EntityEntry parameter)
	{
		var properties = parameter.Metadata.FindPrimaryKey().Verify().Properties;

		switch (properties.Count)
		{
			case 1:
				return parameter.Property(properties[0].Name).CurrentValue.Verify();
			default:
				return properties.Select(p => parameter.Property(p.Name).CurrentValue.Verify()).ToArray();
		}
	}
}