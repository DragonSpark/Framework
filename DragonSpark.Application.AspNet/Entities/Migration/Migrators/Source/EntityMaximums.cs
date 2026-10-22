using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

sealed class EntityMaximums : ConcurrentTable<IEntityType, object>
{
	public static EntityMaximums Default { get; } = new();

	EntityMaximums() {}
}