using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class Keys : ISelect<EntityEntry, object>
{
	public static Keys Default { get; } = new();

	Keys() {}

	public object Get(EntityEntry entry)
	{
		var properties = entry.Metadata.FindPrimaryKey().Verify().Properties;
		if (properties.Count == 1)
		{
			return entry.Property(properties[0].Name).CurrentValue.Verify();
		}

		var result = new object[properties.Count];
		for (var i = 0; i < properties.Count; i++)
		{
			result[i] = entry.Property(properties[i].Name).CurrentValue.Verify();
		}
		return result;
	}
}